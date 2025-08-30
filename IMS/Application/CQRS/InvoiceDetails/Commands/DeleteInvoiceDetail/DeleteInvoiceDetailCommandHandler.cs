using MediatR;
using Application.Contracts;
using Application.Responses;

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

            await invoiceDetailRepository.DeleteAsync(invoiceDetail, cancellationToken);

            response.Success = true;
            response.Message = "Invoice detail deleted successfully.";
            return response;
        }
    }
}