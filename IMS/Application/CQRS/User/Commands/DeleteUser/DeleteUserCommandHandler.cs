using MediatR;
using Application.Contracts;
using Application.Responses;

namespace Application.CQRS.User.Commands.DeleteUser
{
    public class DeleteUserCommandHandler(IUserRepository userRepository)
        : IRequestHandler<DeleteUserCommand, BaseCommandResponse>
    {
        public async Task<BaseCommandResponse> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();
            var user = await userRepository.GetByIdAsync(request.Id, cancellationToken);

            if (user == null)
            {
                response.Success = false;
                response.Message = "User not found for deactivation.";
                return response;
            }

            // 💡 የተጠቃሚውን ሁኔታ ወደ ስራ የማያገለግል (Deactivate) ይቀይራል
            user.IsActive = false;
            await userRepository.Update(user, cancellationToken);

            response.Success = true;
            response.Message = "User deactivated successfully.";
            return response;
        }
    }
}