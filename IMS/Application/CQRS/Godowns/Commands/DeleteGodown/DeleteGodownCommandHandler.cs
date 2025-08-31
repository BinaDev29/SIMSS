using MediatR;
using Application.Contracts;
using Application.Responses;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.Godowns.Commands.DeleteGodown
{
    public class DeleteGodownCommandHandler(IGodownRepository godownRepository, IGodownInventoryRepository godownInventoryRepository)
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

            // 💡 መጋዘኑ ከእቃ ዝርዝሮች ወይም ከግብይቶች ጋር የተገናኘ መሆኑን ማረጋገጥ
            var hasInventory = await godownInventoryRepository.HasInventoryByGodownIdAsync(request.Id, cancellationToken);
            if (hasInventory)
            {
                response.Success = false;
                response.Message = "Cannot delete godown because it is not empty.";
                return response;
            }

            await godownRepository.Delete(godown, cancellationToken);
            response.Success = true;
            response.Message = "Godown deleted successfully.";
            return response;
        }
    }
}