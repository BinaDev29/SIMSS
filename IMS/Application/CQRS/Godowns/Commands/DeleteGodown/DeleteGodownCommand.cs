using Application.Responses;
using MediatR;

namespace Application.CQRS.Godown.Commands.DeleteGodown
{
    public class DeleteGodownCommand : IRequest<BaseCommandResponse>
    {
        public required int Id { get; set; }
    }
}