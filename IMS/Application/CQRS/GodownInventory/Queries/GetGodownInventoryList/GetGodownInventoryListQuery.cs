using Application.DTOs.GodownInventory;
using MediatR;

namespace Application.CQRS.GodownInventory.Queries.GetGodownInventoryList
{
    public class GetGodownInventoryListQuery : IRequest<List<GodownInventoryDto>>
    {
        // Add filtering, paging, sorting parameters if needed
    }
}