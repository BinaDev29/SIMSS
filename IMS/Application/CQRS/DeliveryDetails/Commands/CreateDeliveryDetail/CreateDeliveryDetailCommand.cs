using Application.DTOs.Delivery;
using Application.Responses;
using MediatR;

namespace Application.CQRS.DeliveryDetails.Commands.CreateDeliveryDetail
{
    public class CreateDeliveryDetailCommand : IRequest<BaseCommandResponse>
    {
        public required CreateDeliveryDetailDto DeliveryDetailDto { get; set; }
    }
}