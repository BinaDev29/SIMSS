// GetUserListQuery.cs
using MediatR;
using Application.DTOs.User;
using System.Collections.Generic;

namespace Application.CQRS.User.Queries.GetUserList
{
    public class GetUserListQuery : IRequest<List<UserDto>>
    {
    }
}