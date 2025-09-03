using Application.DTOs.Delivery;
using Application.Responses;
using MediatR;

namespace Application.CQRS.Delivery.Commands.CreateDelivery
{
    public class CreateDeliveryCommand : IRequest<BaseCommandResponse>
    {
        public required CreateDeliveryDto DeliveryDto { get; set; }
    }
}