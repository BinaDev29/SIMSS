using MediatR;
using Application.Contracts;
using Application.DTOs.User;
using AutoMapper;

namespace Application.CQRS.User.Queries.GetUserById
{
    public class GetUserByIdQueryHandler(IUserRepository userRepository, IMapper mapper)
        : IRequestHandler<GetUserByIdQuery, UserDto>
    {
        public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetByIdAsync(request.Id, cancellationToken);
            return mapper.Map<UserDto>(user);
        }
    }
}