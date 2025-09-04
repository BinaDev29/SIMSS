using Application.DTOs.GodownInventory;
using Application.Responses;
using MediatR;
namespace Application.CQRS.GodownInventory.Commands.UpdateGodownInventory
{
    public class UpdateGodownInventoryCommand : IRequest<BaseCommandResponse>
    {
        public required UpdateGodownInventoryDto GodownInventoryDto { get; set; }
    }
}