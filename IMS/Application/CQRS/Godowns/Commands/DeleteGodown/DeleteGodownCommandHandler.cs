using MediatR;
using Application.Contracts;
using Application.Responses;

namespace Application.CQRS.Godown.Commands.DeleteGodown
{
    public class DeleteGodownCommandHandler : IRequestHandler<DeleteGodownCommand, BaseCommandResponse>
    {
        private readonly IGodownRepository _godownRepository;

        public DeleteGodownCommandHandler(IGodownRepository godownRepository)
        {
            _godownRepository = godownRepository;
        }

        public async Task<BaseCommandResponse> Handle(DeleteGodownCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();
            var godown = await _godownRepository.GetByIdAsync(request.Id, cancellationToken);

            if (godown == null)
            {
                response.Success = false;
                response.Message = "Godown not found for deletion.";
                return response;
            }

            await _godownRepository.DeleteAsync(godown, cancellationToken);
            response.Success = true;
            response.Message = "Godown deleted successfully.";
            return response;
        }
    }
}