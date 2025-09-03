using Application.Contracts;
using Application.DTOs.Item;
using Application.Responses;
using Application.DTOs.Common;
using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.Items.Queries.GetItemList
{
    public class GetItemListQueryHandler(IItemRepository itemRepository, IMapper mapper)
        : IRequestHandler<GetItemListQuery, PagedResponse<ItemDto>>
    {
        public async Task<PagedResponse<ItemDto>> Handle(GetItemListQuery request, CancellationToken cancellationToken)
        {
            // 💡 በገጽ የተከፋፈለ እና የተጣራ የእቃዎች ዝርዝር ያመጣል
            var pagedResult = await itemRepository.GetPagedItemsAsync(
                request.Parameters.PageNumber,
                request.Parameters.PageSize,
                request.Parameters.SearchTerm,
                cancellationToken);

            var itemDtos = mapper.Map<List<ItemDto>>(pagedResult.Items);

            return new PagedResponse<ItemDto>(
                itemDtos,
                pagedResult.TotalCount,
                pagedResult.PageNumber,
                pagedResult.PageSize);
        }
    }
}