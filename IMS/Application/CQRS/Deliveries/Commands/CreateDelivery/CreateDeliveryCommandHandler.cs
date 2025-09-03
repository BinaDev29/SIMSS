using MediatR;
using Application.Contracts;
using Application.DTOs.Delivery.Validators;
using Application.Responses;
using AutoMapper;

namespace Application.CQRS.Delivery.Commands.CreateDelivery
{
    public class CreateDeliveryCommandHandler : IRequestHandler<CreateDeliveryCommand, BaseCommandResponse>
    {
        private readonly IDeliveryRepository _deliveryRepository;
        private readonly IMapper _mapper;

        public CreateDeliveryCommandHandler(IDeliveryRepository deliveryRepository, IMapper mapper)
        {
            _deliveryRepository = deliveryRepository;
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
                response.Errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return response;
            }

            var delivery = _mapper.Map<Domain.Models.Delivery>(request.DeliveryDto);
            var addedDelivery = await _deliveryRepository.AddAsync(delivery, cancellationToken);

            response.Success = true;
            response.Message = "Delivery created successfully.";
            response.Id = addedDelivery.Id;

            return response;
        }
    }
}