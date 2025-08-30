using MediatR;
using Application.Contracts;
using Application.Responses;

namespace Application.CQRS.Godowns.Commands.DeleteGodown
{
    public class DeleteGodownCommandHandler(IGodownRepository godownRepository)
        : IRequestHandler<DeleteGodownCommand, BaseCommandResponse>
    {
        public async Task<BaseCommandResponse> Handle(DeleteGodownCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();
            var godown = await godownRepository.GetByIdAsync(request.Id, cancellationToken);

            if (godown == null)
            {
                response.Success = false;
                response.Message = "Godown not found for deletion.";
                return response;
            }

            await godownRepository.DeleteAsync(godown, cancellationToken);

            response.Success = true;
            response.Message = "Godown deleted successfully.";
            return response;
        }
    }
}