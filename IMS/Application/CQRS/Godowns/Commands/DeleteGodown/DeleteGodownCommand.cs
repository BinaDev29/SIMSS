using MediatR;
using Application.Responses;

namespace Application.CQRS.Godowns.Commands.DeleteGodown
{
    public class DeleteGodownCommand : IRequest<BaseCommandResponse>
    {
        public required int Id { get; set; }
    }
}