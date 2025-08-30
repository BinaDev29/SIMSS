using Application.Contracts;
using Application.DTOs.Item;
using AutoMapper;
using MediatR;

namespace Application.CQRS.Items.Queries.GetItemById
{
    public class GetItemByIdQueryHandler(IItemRepository itemRepository, IMapper mapper)
        : IRequestHandler<GetItemByIdQuery, ItemDto>
    {
        public async Task<ItemDto> Handle(GetItemByIdQuery request, CancellationToken cancellationToken)
        {
            var item = await itemRepository.GetByIdAsync(request.Id, cancellationToken);
            return mapper.Map<ItemDto>(item);
        }
    }
}