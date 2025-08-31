using MediatR;
using Application.DTOs.Item;
using Application.DTOs.Common;
using Application.Responses;

namespace Application.CQRS.Items.Queries.GetItemList
{
    public class GetItemListQuery : IRequest<PagedResponse<ItemDto>>
    {
        public required ItemQueryParameters Parameters { get; set; }
    }
}