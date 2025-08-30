using MediatR;
using Application.DTOs.Delivery;
using Application.Responses;

namespace Application.CQRS.Deliveries.Commands.CreateDelivery
{
    public class CreateDeliveryCommand : IRequest<BaseCommandResponse>
    {
        public required CreateDeliveryDto DeliveryDto { get; set; }
    }
}