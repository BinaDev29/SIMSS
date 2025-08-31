using MediatR;
using Application.Contracts;
using Application.Responses;

namespace Application.CQRS.Suppliers.Commands.DeleteSupplier
{
    public class DeleteSupplierCommandHandler(
        ISupplierRepository supplierRepository,
        IInwardTransactionRepository inwardTransactionRepository)
        : IRequestHandler<DeleteSupplierCommand, BaseCommandResponse>
    {
        public async Task<BaseCommandResponse> Handle(DeleteSupplierCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();
            var supplier = await supplierRepository.GetByIdAsync(request.Id, cancellationToken);

            if (supplier == null)
            {
                response.Success = false;
                response.Message = "Supplier not found for deletion.";
                return response;
            }

            // 💡 አቅራቢው ከገቢ ግብይቶች ጋር የተገናኘ መሆኑን ማረጋገጥ
            var hasInwardTransactions = await inwardTransactionRepository.HasTransactionsBySupplierIdAsync(request.Id, cancellationToken);
            if (hasInwardTransactions)
            {
                response.Success = false;
                response.Message = "Cannot delete supplier because they are associated with transactions.";
                return response;
            }

            await supplierRepository.Delete(supplier, cancellationToken);
            response.Success = true;
            response.Message = "Supplier deleted successfully.";
            return response;
        }
    }
}