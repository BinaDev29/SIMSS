using MediatR;
using Application.Contracts;
using Application.Responses;

namespace Application.CQRS.Transactions.Commands.DeleteReturnTransaction
{
    public class DeleteReturnTransactionCommandHandler(IReturnTransactionRepository returnTransactionRepository, IItemRepository itemRepository)
        : IRequestHandler<DeleteReturnTransactionCommand, BaseCommandResponse>
    {
        public async Task<BaseCommandResponse> Handle(DeleteReturnTransactionCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();
            var transactionToDelete = await returnTransactionRepository.GetByIdAsync(request.Id, cancellationToken);

            if (transactionToDelete == null)
            {
                response.Success = false;
                response.Message = "Return Transaction not found.";
                return response;
            }

            // Revert stock change
            var item = await itemRepository.GetByIdAsync(transactionToDelete.ItemId, cancellationToken);
            if (item != null)
            {
                item.StockQuantity -= transactionToDelete.Quantity;
                await itemRepository.UpdateAsync(item, cancellationToken);
            }

            await returnTransactionRepository.DeleteAsync(transactionToDelete, cancellationToken);

            response.Success = true;
            response.Message = "Return Transaction deleted successfully.";
            return response;
        }
    }
}