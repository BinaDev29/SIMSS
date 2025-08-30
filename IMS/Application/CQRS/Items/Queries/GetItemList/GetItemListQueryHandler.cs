using Application.Contracts;
using Application.DTOs.Item;
using AutoMapper;
using MediatR;
using System.Collections.Generic;

namespace Application.CQRS.Items.Queries.GetItemList
{
    public class GetItemListQueryHandler(IItemRepository itemRepository, IMapper mapper)
        : IRequestHandler<GetItemListQuery, List<ItemDto>>
    {
        public async Task<List<ItemDto>> Handle(GetItemListQuery request, CancellationToken cancellationToken)
        {
            var items = await itemRepository.GetAllAsync(cancellationToken);
            return mapper.Map<List<ItemDto>>(items);
        }
    }
}