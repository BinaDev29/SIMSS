// DeleteItemCommand.cs
using MediatR;
using Application.Responses;

namespace Application.CQRS.Items.Commands.DeleteItem
{
    public class DeleteItemCommand : IRequest<BaseCommandResponse>
    {
        public required int Id { get; set; }
    }
}