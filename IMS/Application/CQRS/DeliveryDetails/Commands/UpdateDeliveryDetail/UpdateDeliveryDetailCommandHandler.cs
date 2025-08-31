using Application.Contracts;
using Application.DTOs.Delivery;
using Application.DTOs.Delivery.Validators;
using Application.Responses;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Application.CQRS.DeliveryDetails.Commands.UpdateDeliveryDetail
{
    public class UpdateDeliveryDetailCommandHandler(IDeliveryDetailRepository deliveryDetailRepository, IMapper mapper, IValidator<UpdateDeliveryDetailDto> validator)
        : IRequestHandler<UpdateDeliveryDetailCommand, BaseCommandResponse>
    {
        public async Task<BaseCommandResponse> Handle(UpdateDeliveryDetailCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();
            var validationResult = await validator.ValidateAsync(request.DeliveryDetailDto, cancellationToken);

            if (!validationResult.IsValid)
            {
                response.Success = false;
                response.Message = "DeliveryDetail update failed due to validation errors.";
                response.Errors = validationResult.Errors.Select(q => q.ErrorMessage).ToList();
                return response;
            }

            var deliveryDetail = await deliveryDetailRepository.GetByIdAsync(request.Id, cancellationToken);
            if (deliveryDetail == null)
            {
                response.Success = false;
                response.Message = "DeliveryDetail not found.";
                return response;
            }

            mapper.Map(request.DeliveryDetailDto, deliveryDetail);
            await deliveryDetailRepository.Update(deliveryDetail, cancellationToken);

            response.Success = true;
            response.Message = "DeliveryDetail updated successfully.";
            response.Id = deliveryDetail.Id;
            return response;
        }
    }
}