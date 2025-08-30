using MediatR;
using Application.Contracts;
using Application.DTOs.Delivery.Validators;
using Application.Responses;
using AutoMapper;
using Domain.Models;

namespace Application.CQRS.Deliveries.Commands.CreateDelivery
{
    public class CreateDeliveryCommandHandler(IDeliveryRepository deliveryRepository, IMapper mapper)
        : IRequestHandler<CreateDeliveryCommand, BaseCommandResponse>
    {
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

            var delivery = mapper.Map<Domain.Models.Delivery>(request.DeliveryDto);
            var addedDelivery = await deliveryRepository.AddAsync(delivery, cancellationToken);

            response.Success = true;
            response.Message = "Delivery created successfully.";
            response.Id = addedDelivery.Id;

            return response;
        }
    }
}