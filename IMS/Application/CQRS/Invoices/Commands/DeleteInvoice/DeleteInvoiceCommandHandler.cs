using MediatR;
using Application.Contracts;
using Application.Responses;
using Domain.Models;
using System.Collections.Generic;

namespace Application.CQRS.Invoices.Commands.DeleteInvoice
{
    public class DeleteInvoiceCommandHandler(IInvoiceRepository invoiceRepository, IItemRepository itemRepository, IInvoiceDetailRepository invoiceDetailRepository)
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

            // Return stock for all items in the invoice
            var invoiceDetails = await invoiceDetailRepository.GetInvoiceDetailsByInvoiceIdAsync(invoice.Id, cancellationToken);
            foreach (var detail in invoiceDetails)
            {
                var item = await itemRepository.GetByIdAsync(detail.ItemId, cancellationToken);
                if (item != null)
                {
                    item.StockQuantity += detail.Quantity;
                    await itemRepository.UpdateAsync(item, cancellationToken);
                }
            }

            // Delete invoice details and then the invoice
            await invoiceDetailRepository.DeleteRangeAsync(invoiceDetails, cancellationToken);
            await invoiceRepository.DeleteAsync(invoice, cancellationToken);

            response.Success = true;
            response.Message = "Invoice deleted successfully.";
            return response;
        }
    }
}