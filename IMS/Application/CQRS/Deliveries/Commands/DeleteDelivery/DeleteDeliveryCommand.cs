using Application.Responses;
using MediatR;

namespace Application.CQRS.Delivery.Commands.DeleteDelivery
{
    public class DeleteDeliveryCommand : IRequest<BaseCommandResponse>
    {
        public required int Id { get; set; }
    }
}