// Application/CQRS/Deliveries/Commands/CreateDelivery/CreateDeliveryCommandHandler.cs
using MediatR;
using Application.Contracts;
using Application.DTOs.Delivery.Validators;
using Application.Responses;
using AutoMapper;
using Domain.Models;
using Application.DTOs.Delivery;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.Deliveries.Commands.CreateDelivery
{
    public class CreateDeliveryCommandHandler : IRequestHandler<CreateDeliveryCommand, BaseCommandResponse>
    {
        private readonly IDeliveryRepository _deliveryRepository;
        private readonly IOutwardTransactionRepository _outwardTransactionRepository;
        private readonly IMapper _mapper;

        // Primary constructor
        public CreateDeliveryCommandHandler(IDeliveryRepository deliveryRepository, IOutwardTransactionRepository outwardTransactionRepository, IMapper mapper)
        {
            _deliveryRepository = deliveryRepository;
            _outwardTransactionRepository = outwardTransactionRepository;
            _mapper = mapper;
        }

        public async Task<BaseCommandResponse> Handle(CreateDeliveryCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();
            var validator = new CreateDeliveryValidator();
            var validationResult = await validator.ValidateAsync(request.DeliveryDto, cancellationToken);

            if (!validationResult.IsValid)
            {
                response.Success = false;
                response.Message = "Delivery creation failed due to validation errors.";
                response.Errors = validationResult.Errors.Select(q => q.ErrorMessage).ToList();
                return response;
            }

            // 💡 1. ከተጠቀሰው OutwardTransaction ጋር የተያያዘ መላኪያ መኖሩን ማረጋገጥ
            var transactionExists = await _outwardTransactionRepository.GetByIdAsync(request.DeliveryDto.OutwardTransactionId, cancellationToken);
            if (transactionExists == null)
            {
                response.Success = false;
                response.Message = "The specified outward transaction does not exist.";
                return response;
            }

            // 💡 2. OutwardTransactionው ቀድሞውንም Delivery እንደሌለው ማረጋገጥ
            var existingDelivery = await _deliveryRepository.GetByOutwardTransactionIdAsync(request.DeliveryDto.OutwardTransactionId, cancellationToken);
            if (existingDelivery != null)
            {
                response.Success = false;
                response.Message = "A delivery record for this transaction already exists.";
                return response;
            }

            var delivery = _mapper.Map<Domain.Models.Delivery>(request.DeliveryDto);
            var addedDelivery = await _deliveryRepository.AddAsync(delivery, cancellationToken); // Ensure AddAsync returns the added entity

            response.Success = true;
            response.Message = "Delivery created successfully.";
            response.Id = addedDelivery.Id; // Use the Id from the added delivery object
            return response;
        }
    }
}
