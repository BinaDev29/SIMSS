using MediatR;
using Application.Contracts;
using Application.Responses;

namespace Application.CQRS.GodownInventory.Commands.DeleteGodownInventory
{
    public class DeleteGodownInventoryCommandHandler : IRequestHandler<DeleteGodownInventoryCommand, BaseCommandResponse>
    {
        private readonly IGodownInventoryRepository _godownInventoryRepository;

        public DeleteGodownInventoryCommandHandler(IGodownInventoryRepository godownInventoryRepository)
        {
            _godownInventoryRepository = godownInventoryRepository;
        }

        public async Task<BaseCommandResponse> Handle(DeleteGodownInventoryCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();

            var inventory = await _godownInventoryRepository.GetByIdAsync(request.Id, cancellationToken);
            if (inventory == null)
            {
                response.Success = false;
                response.Message = "Godown inventory not found for deletion.";
                return response;
            }

            await _godownInventoryRepository.DeleteAsync(inventory, cancellationToken);
            response.Success = true;
            response.Message = "Godown inventory deleted successfully.";
            return response;
        }
    }
}