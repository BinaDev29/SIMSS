using Application.DTOs.Delivery;
using Application.Responses;
using MediatR;

namespace Application.CQRS.Delivery.Commands.UpdateDelivery
{
    public class UpdateDeliveryCommand : IRequest<BaseCommandResponse>
    {
        public required UpdateDeliveryDto DeliveryDto { get; set; }
    }
}