using MediatR;
using Application.DTOs.User;
using Application.Responses;

namespace Application.CQRS.User.Commands.UpdateUser
{
    public class UpdateUserCommand : IRequest<BaseCommandResponse>
    {
        public int Id { get; set; }
        public required UpdateUserDto UserDto { get; set; }
    }
}