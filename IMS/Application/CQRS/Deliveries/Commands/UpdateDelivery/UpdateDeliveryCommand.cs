using MediatR;
using Application.DTOs.Delivery;
using Application.Responses;

namespace Application.CQRS.Deliveries.Commands.UpdateDelivery
{
    public class UpdateDeliveryCommand : IRequest<BaseCommandResponse>
    {
        public int Id { get; set; }
        public required UpdateDeliveryDto DeliveryDto { get; set; }
    }
}