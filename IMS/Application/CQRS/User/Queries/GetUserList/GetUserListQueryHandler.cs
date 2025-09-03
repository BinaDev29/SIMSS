using MediatR;
using Application.Contracts;
using Application.DTOs.User;
using Application.Responses;
using Application.DTOs.Common;
using AutoMapper;

namespace Application.CQRS.User.Queries.GetUserList
{
    public class GetUserListQueryHandler(IUserRepository userRepository, IMapper mapper)
        : IRequestHandler<GetUserListQuery, PagedResponse<UserDto>>
    {
        public async Task<PagedResponse<UserDto>> Handle(GetUserListQuery request, CancellationToken cancellationToken)
        {
            // 💡 በገጽ የተከፋፈለ እና የተጣራ የተጠቃሚዎች ዝርዝር ያመጣል
            var pagedResult = await userRepository.GetPagedUsersAsync(
                request.Parameters.PageNumber,
                request.Parameters.PageSize,
                request.Parameters.SearchTerm,
                cancellationToken);

            var userDtos = mapper.Map<List<UserDto>>(pagedResult.Items);

            return new PagedResponse<UserDto>(
                userDtos,
                pagedResult.TotalCount,
                pagedResult.PageNumber,
                pagedResult.PageSize);
        }
    }
}