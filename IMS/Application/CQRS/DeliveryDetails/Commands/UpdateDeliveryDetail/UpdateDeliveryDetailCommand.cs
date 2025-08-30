using Application.DTOs.Delivery;
using Application.Responses;
using MediatR;

namespace Application.CQRS.DeliveryDetails.Commands.UpdateDeliveryDetail
{
    public class UpdateDeliveryDetailCommand : IRequest<BaseCommandResponse>
    {
        public int Id { get; set; }
        public required UpdateDeliveryDetailDto DeliveryDetailDto { get; set; }
    }
}