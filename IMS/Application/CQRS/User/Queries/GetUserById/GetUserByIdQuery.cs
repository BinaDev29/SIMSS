// GetUserByIdQuery.cs
using MediatR;
using Application.DTOs.User;

namespace Application.CQRS.User.Queries.GetUserById
{
    public class GetUserByIdQuery : IRequest<UserDto>
    {
        public required int Id { get; set; }
    }
}