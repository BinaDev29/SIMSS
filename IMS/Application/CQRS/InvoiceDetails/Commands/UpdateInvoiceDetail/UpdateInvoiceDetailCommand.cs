using MediatR;
using Application.DTOs.Invoice;
using Application.Responses;

namespace Application.CQRS.InvoiceDetails.Commands.UpdateInvoiceDetail
{
    public class UpdateInvoiceDetailCommand : IRequest<BaseCommandResponse>
    {
        public int Id { get; set; }
        public required UpdateInvoiceDetailDto InvoiceDetailDto { get; set; }
    }
}