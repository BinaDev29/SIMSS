// GetItemByIdQuery.cs
using Application.DTOs.Item;
using MediatR;

namespace Application.CQRS.Items.Queries.GetItemById
{
    public class GetItemByIdQuery : IRequest<ItemDto>
    {
        public required int Id { get; set; }
    }
}