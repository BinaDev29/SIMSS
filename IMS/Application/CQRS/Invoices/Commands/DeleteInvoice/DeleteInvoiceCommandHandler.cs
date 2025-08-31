using MediatR;
using Application.Contracts;
using Application.Responses;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.Invoices.Commands.DeleteInvoice
{
    public class DeleteInvoiceCommandHandler(IInvoiceRepository invoiceRepository)
        : IRequestHandler<DeleteInvoiceCommand, BaseCommandResponse>
    {
        public async Task<BaseCommandResponse> Handle(DeleteInvoiceCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();
            var invoice = await invoiceRepository.GetByIdAsync(request.Id, cancellationToken);

            if (invoice == null)
            {
                response.Success = false;
                response.Message = "Invoice not found for deletion.";
                return response;
            }

            // 💡 የክፍያ መጠየቂያዎችን መሰረዝ አይፈቀድም
            response.Success = false;
            response.Message = "Deleting invoices is not allowed to maintain financial data integrity.";
            return response;
        }
    }
}