// CreateUserCommand.cs
using MediatR;
using Application.DTOs.User;
using Application.Responses;

namespace Application.CQRS.User.Commands.CreateUser
{
    public class CreateUserCommand : IRequest<BaseCommandResponse>
    {
        public required CreateUserDto UserDto { get; set; }
    }
}