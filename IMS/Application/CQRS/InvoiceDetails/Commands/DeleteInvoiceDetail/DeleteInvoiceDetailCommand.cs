using MediatR;
using Application.Responses;

namespace Application.CQRS.InvoiceDetails.Commands.DeleteInvoiceDetail
{
    public class DeleteInvoiceDetailCommand : IRequest<BaseCommandResponse>
    {
        public required int Id { get; set; }
    }
}