using MediatR;
using Application.DTOs.Invoice;
using Application.Responses;

namespace Application.CQRS.Invoices.Commands.UpdateInvoice
{
    public class UpdateInvoiceCommand : IRequest<BaseCommandResponse>
    {
        public int Id { get; set; }
        public required UpdateInvoiceDto InvoiceDto { get; set; }
    }
}