using MediatR;
using Application.Contracts;
using Application.Responses;

namespace Application.CQRS.Transactions.Commands.DeleteInwardTransaction
{
    public class DeleteInwardTransactionCommandHandler(IInwardTransactionRepository inwardTransactionRepository, IItemRepository itemRepository)
        : IRequestHandler<DeleteInwardTransactionCommand, BaseCommandResponse>
    {
        public async Task<BaseCommandResponse> Handle(DeleteInwardTransactionCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();
            var transactionToDelete = await inwardTransactionRepository.GetByIdAsync(request.Id, cancellationToken);

            if (transactionToDelete == null)
            {
                response.Success = false;
                response.Message = "Inward Transaction not found.";
                return response;
            }

            // Revert stock change
            var item = await itemRepository.GetByIdAsync(transactionToDelete.ItemId, cancellationToken);
            if (item != null)
            {
                item.StockQuantity -= transactionToDelete.QuantityReceived;
                await itemRepository.UpdateAsync(item, cancellationToken);
            }

            await inwardTransactionRepository.DeleteAsync(transactionToDelete, cancellationToken);

            response.Success = true;
            response.Message = "Inward Transaction deleted successfully.";
            return response;
        }
    }
}