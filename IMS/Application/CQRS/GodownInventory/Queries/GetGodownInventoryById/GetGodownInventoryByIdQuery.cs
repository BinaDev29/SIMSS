using Application.DTOs.GodownInventory;
using MediatR;

namespace Application.CQRS.GodownInventory.Queries.GetGodownInventoryById
{
    public class GetGodownInventoryByIdQuery : IRequest<GodownInventoryDto?>
    {
        public required int Id { get; set; }
    }
}