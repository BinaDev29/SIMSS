using MediatR;
using Application.Contracts;
using Application.Responses;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.Items.Commands.DeleteItem
{
    public class DeleteItemCommandHandler(
        IItemRepository itemRepository,
        IGodownInventoryRepository godownInventoryRepository,
        IInwardTransactionRepository inwardTransactionRepository,
        IOutwardTransactionRepository outwardTransactionRepository,
        IInvoiceDetailRepository invoiceDetailRepository)
        : IRequestHandler<DeleteItemCommand, BaseCommandResponse>
    {
        public async Task<BaseCommandResponse> Handle(DeleteItemCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();
            var item = await itemRepository.GetByIdAsync(request.Id, cancellationToken);

            if (item == null)
            {
                response.Success = false;
                response.Message = "Item not found for deletion.";
                return response;
            }

            // 💡 እቃው ከሌሎች መዝገቦች ጋር የተገናኘ መሆኑን ማረጋገጥ
            var hasInventory = await godownInventoryRepository.HasInventoryByItemIdAsync(request.Id, cancellationToken);
            var hasInwardTransactions = await inwardTransactionRepository.HasTransactionsByItemIdAsync(request.Id, cancellationToken);
            var hasOutwardTransactions = await outwardTransactionRepository.HasTransactionsByItemIdAsync(request.Id, cancellationToken);
            var hasInvoiceDetails = await invoiceDetailRepository.HasDetailsByItemIdAsync(request.Id, cancellationToken);

            if (hasInventory || hasInwardTransactions || hasOutwardTransactions || hasInvoiceDetails)
            {
                response.Success = false;
                response.Message = "Cannot delete item because it is associated with inventory, transactions, or invoices.";
                return response;
            }

            await itemRepository.DeleteAsync(item, cancellationToken);
            response.Success = true;
            response.Message = "Item deleted successfully.";
            return response;
        }
    }
}