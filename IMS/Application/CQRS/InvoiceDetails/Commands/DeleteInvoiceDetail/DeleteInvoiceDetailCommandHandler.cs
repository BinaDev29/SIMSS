using MediatR;
using Application.Contracts;
using Application.Responses;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.InvoiceDetails.Commands.DeleteInvoiceDetail
{
    public class DeleteInvoiceDetailCommandHandler(IInvoiceDetailRepository invoiceDetailRepository)
        : IRequestHandler<DeleteInvoiceDetailCommand, BaseCommandResponse>
    {
        public async Task<BaseCommandResponse> Handle(DeleteInvoiceDetailCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();
            var invoiceDetail = await invoiceDetailRepository.GetByIdAsync(request.Id, cancellationToken);

            if (invoiceDetail == null)
            {
                response.Success = false;
                response.Message = "Invoice detail not found for deletion.";
                return response;
            }

            // 💡 የክፍያ መጠየቂያ ዝርዝሮችን መሰረዝ የፋይናንስ መዝገቦችን ስለሚጎዳ አይፈቀድም።
            response.Success = false;
            response.Message = "Deleting invoice details is not allowed to maintain financial data integrity.";
            return response;
        }
    }
}