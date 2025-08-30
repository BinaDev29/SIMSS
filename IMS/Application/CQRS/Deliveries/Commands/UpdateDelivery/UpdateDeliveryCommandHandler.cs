using Application.Contracts;
using Application.DTOs.Delivery;
using Application.DTOs.Delivery.Validators;
using Application.Responses;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Application.CQRS.Deliveries.Commands.UpdateDelivery
{
    public class UpdateDeliveryCommandHandler(IDeliveryRepository deliveryRepository, IMapper mapper, IValidator<UpdateDeliveryDto> validator)
        : IRequestHandler<UpdateDeliveryCommand, BaseCommandResponse>
    {
        public async Task<BaseCommandResponse> Handle(UpdateDeliveryCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();
            var validationResult = await validator.ValidateAsync(request.DeliveryDto, cancellationToken);

            if (!validationResult.IsValid)
            {
                response.Success = false;
                response.Message = "Delivery update failed due to validation errors.";
                response.Errors = validationResult.Errors.Select(q => q.ErrorMessage).ToList();
                return response;
            }

            var delivery = await deliveryRepository.GetByIdAsync(request.Id, cancellationToken);
            if (delivery == null)
            {
                response.Success = false;
                response.Message = "Delivery not found.";
                return response;
            }

            mapper.Map(request.DeliveryDto, delivery);
            await deliveryRepository.UpdateAsync(delivery, cancellationToken);

            response.Success = true;
            response.Message = "Delivery updated successfully.";
            response.Id = delivery.Id;
            return response;
        }
    }
}