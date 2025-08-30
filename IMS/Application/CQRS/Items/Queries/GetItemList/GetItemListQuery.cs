// GetItemListQuery.cs
using Application.DTOs.Item;
using MediatR;
using System.Collections.Generic;

namespace Application.CQRS.Items.Queries.GetItemList
{
    public class GetItemListQuery : IRequest<List<ItemDto>>
    {
    }
}