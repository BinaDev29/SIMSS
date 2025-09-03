// CreateInvoiceCommand.cs
using MediatR;
using Application.DTOs.Invoice;
using Application.Responses;

namespace Application.CQRS.Invoices.Commands.CreateInvoice
{
    public class CreateInvoiceCommand : IRequest<BaseCommandResponse>
    {
        public required CreateInvoiceDto InvoiceDto { get; set; }
    }
}