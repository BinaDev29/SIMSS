using MediatR;
using Application.Responses;

namespace Application.CQRS.DeliveryDetails.Commands.DeleteDeliveryDetail
{
    public class DeleteDeliveryDetailCommand : IRequest<BaseCommandResponse>
    {
        public required int Id { get; set; }
    }
}