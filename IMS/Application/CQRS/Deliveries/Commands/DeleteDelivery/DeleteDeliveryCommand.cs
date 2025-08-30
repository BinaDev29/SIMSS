using MediatR;
using Application.Responses;

namespace Application.CQRS.Deliveries.Commands.DeleteDelivery
{
    public class DeleteDeliveryCommand : IRequest<BaseCommandResponse>
    {
        public required int Id { get; set; }
    }
}