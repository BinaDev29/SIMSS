using Application.Contracts;
using Application.DTOs.User;
using Application.DTOs.User.Validators;
using Application.Responses;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Application.CQRS.User.Commands.UpdateUser
{
    public class UpdateUserCommandHandler(IUserRepository userRepository, IMapper mapper, IValidator<UpdateUserDto> validator)
        : IRequestHandler<UpdateUserCommand, BaseCommandResponse>
    {
        public async Task<BaseCommandResponse> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();
            var validationResult = await validator.ValidateAsync(request.UserDto, cancellationToken);

            if (!validationResult.IsValid)
            {
                response.Success = false;
                response.Message = "User update failed due to validation errors.";
                response.Errors = validationResult.Errors.Select(q => q.ErrorMessage).ToList();
                return response;
            }

            var user = await userRepository.GetByIdAsync(request.Id, cancellationToken);
            if (user == null)
            {
                response.Success = false;
                response.Message = "User not found.";
                return response;
            }

            mapper.Map(request.UserDto, user);
            await userRepository.UpdateAsync(user, cancellationToken);

            response.Success = true;
            response.Message = "User updated successfully.";
            response.Id = user.Id;
            return response;
        }
    }
}