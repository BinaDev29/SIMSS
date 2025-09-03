using MediatR;
using Application.Contracts;
using Application.DTOs.User;
using Application.DTOs.User.Validators;
using Application.Responses;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace Application.CQRS.User.Commands.CreateUser
{
    public class CreateUserCommandHandler(
        IUserRepository userRepository,
        IMapper mapper,
        IPasswordHasher<Domain.Models.User> passwordHasher)
        : IRequestHandler<CreateUserCommand, BaseCommandResponse>
    {
        public async Task<BaseCommandResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();
            var validator = new CreateUserValidator();
            var validationResult = await validator.ValidateAsync(request.UserDto, cancellationToken);

            if (!validationResult.IsValid)
            {
                response.Success = false;
                response.Message = "User creation failed due to validation errors.";
                response.Errors = validationResult.Errors.Select(q => q.ErrorMessage).ToList();
                return response;
            }

            // 💡 ተመሳሳይ የተጠቃሚ ስም ወይም ኢሜይል መኖሩን ማረጋገጥ
            var userExists = await userRepository.GetUserByUsernameAsync(request.UserDto.Username, cancellationToken);
            if (userExists != null)
            {
                response.Success = false;
                response.Message = "A user with this username or email already exists.";
                return response;
            }

            var user = mapper.Map<Domain.Models.User>(request.UserDto);

            // 💡 የይለፍ ቃሉን መስጠር (Hash)
            user.PasswordHash = passwordHasher.HashPassword(user, request.UserDto.PasswordHash);

            user.Role = request.UserDto.Role;
            var addedUser = await userRepository.AddAsync(user, cancellationToken);

            response.Success = true;
            response.Message = "User created successfully.";
            response.Id = addedUser.Id;

            return response;
        }
    }
}