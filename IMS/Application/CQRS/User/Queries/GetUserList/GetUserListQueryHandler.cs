using MediatR;
using Application.Contracts;
using Application.DTOs.User;
using AutoMapper;
using System.Collections.Generic;

namespace Application.CQRS.User.Queries.GetUserList
{
    public class GetUserListQueryHandler(IUserRepository userRepository, IMapper mapper)
        : IRequestHandler<GetUserListQuery, List<UserDto>>
    {
        public async Task<List<UserDto>> Handle(GetUserListQuery request, CancellationToken cancellationToken)
        {
            var users = await userRepository.GetAllAsync(cancellationToken);
            return mapper.Map<List<UserDto>>(users);
        }
    }
}