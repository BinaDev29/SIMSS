using MediatR;
using Application.Contracts;
using Application.Responses;

namespace Application.CQRS.Transactions.Commands.DeleteOutwardTransaction
{
    public class DeleteOutwardTransactionCommandHandler(IOutwardTransactionRepository outwardTransactionRepository, IItemRepository itemRepository)
        : IRequestHandler<DeleteOutwardTransactionCommand, BaseCommandResponse>
    {
        public async Task<BaseCommandResponse> Handle(DeleteOutwardTransactionCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();
            var transactionToDelete = await outwardTransactionRepository.GetByIdAsync(request.Id, cancellationToken);

            if (transactionToDelete == null)
            {
                response.Success = false;
                response.Message = "Outward Transaction not found.";
                return response;
            }

            // Revert stock change
            var item = await itemRepository.GetByIdAsync(transactionToDelete.ItemId, cancellationToken);
            if (item != null)
            {
                item.StockQuantity += transactionToDelete.QuantityDelivered;
                await itemRepository.UpdateAsync(item, cancellationToken);
            }

            await outwardTransactionRepository.DeleteAsync(transactionToDelete, cancellationToken);

            response.Success = true;
            response.Message = "Outward Transaction deleted successfully.";
            return response;
        }
    }
}