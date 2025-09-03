using Application.Responses;
using MediatR;

namespace Application.CQRS.GodownInventory.Commands.DeleteGodownInventory
{
    public class DeleteGodownInventoryCommand : IRequest<BaseCommandResponse>
    {
        public required int Id { get; set; }
    }
}