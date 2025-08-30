using MediatR;
using Application.Contracts;
using Application.Responses;

namespace Application.CQRS.Suppliers.Commands.DeleteSupplier
{
    public class DeleteSupplierCommandHandler(ISupplierRepository supplierRepository)
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

            await supplierRepository.DeleteAsync(supplier, cancellationToken);

            response.Success = true;
            response.Message = "Supplier deleted successfully.";
            return response;
        }
    }
}