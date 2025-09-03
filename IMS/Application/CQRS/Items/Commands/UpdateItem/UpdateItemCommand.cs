using Application.DTOs.Item;
using Application.Responses;
using MediatR;

namespace Application.CQRS.Items.Commands.UpdateItem
{
    public class UpdateItemCommand : IRequest<BaseCommandResponse>
    {
        public int Id { get; set; }
        public required UpdateItemDto ItemDto { get; set; }
    }
}