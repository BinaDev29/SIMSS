// CreateItemCommand.cs
using Application.DTOs.Item;
using Application.Responses;
using MediatR;

namespace Application.CQRS.Items.Commands.CreateItem
{
    public class CreateItemCommand : IRequest<BaseCommandResponse>
    {
        public required CreateItemDto ItemDto { get; set; }
    }
}