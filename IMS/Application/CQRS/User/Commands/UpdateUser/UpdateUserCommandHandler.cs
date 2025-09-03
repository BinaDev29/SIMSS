using MediatR;
using Application.Contracts;
using Application.DTOs.User;
using Application.DTOs.User.Validators;
using Application.Responses;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace Application.CQRS.User.Commands.UpdateUser
{
    public class UpdateUserCommandHandler(
        IUserRepository userRepository,
        IMapper mapper,
        IValidator<UpdateUserDto> validator)
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

            var userToUpdate = await userRepository.GetByIdAsync(request.Id, cancellationToken);
            if (userToUpdate == null)
            {
                response.Success = false;
                response.Message = "User not found for update.";
                return response;
            }

            // 💡 ከአሁኑ የተለየ ስም ከሌላ ተጠቃሚ ጋር ተመሳሳይ መሆኑን ማረጋገጥ
            var existingUser = await userRepository.GetUserByUsernameAsync(request.UserDto.Username, cancellationToken);
            if (existingUser != null && existingUser.Id != request.Id)
            {
                response.Success = false;
                response.Message = "A user with this username or email already exists.";
                return response;
            }

            // 💡 የይለፍ ቃሉን ከDTO ውስጥ አያስተላልፍም
            mapper.Map(request.UserDto, userToUpdate);

            await userRepository.Update(userToUpdate, cancellationToken);

            response.Success = true;
            response.Message = "User updated successfully.";
            response.Id = userToUpdate.Id;

            return response;
        }
    }
}