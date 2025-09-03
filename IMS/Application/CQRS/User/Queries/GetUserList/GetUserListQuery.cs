using MediatR;
using Application.DTOs.User;
using Application.DTOs.Common;
using Application.Responses;

namespace Application.CQRS.User.Queries.GetUserList
{
    public class GetUserListQuery : IRequest<PagedResponse<UserDto>>
    {
        public required UserQueryParameters Parameters { get; set; }
    }
}