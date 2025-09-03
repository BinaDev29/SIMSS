// DeleteUserCommand.cs
using MediatR;
using Application.Responses;

namespace Application.CQRS.User.Commands.DeleteUser
{
    public class DeleteUserCommand : IRequest<BaseCommandResponse>
    {
        public required int Id { get; set; }
    }
}