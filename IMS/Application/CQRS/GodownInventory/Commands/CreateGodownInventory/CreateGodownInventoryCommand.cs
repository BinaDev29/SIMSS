using Application.DTOs.GodownInventory;
using Application.Responses;
using MediatR;

namespace Application.CQRS.GodownInventory.Commands.CreateGodownInventory
{
    public class CreateGodownInventoryCommand : IRequest<BaseCommandResponse>
    {
        public required CreateGodownInventoryDto GodownInventoryDto { get; set; }
    }
}