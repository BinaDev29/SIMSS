using MediatR;
using Application.DTOs.Invoice;
using Application.Responses;

namespace Application.CQRS.InvoiceDetails.Commands.CreateInvoiceDetail
{
    public class CreateInvoiceDetailCommand : IRequest<BaseCommandResponse>
    {
        public required CreateInvoiceDetailDto InvoiceDetailDto { get; set; }
    }
}