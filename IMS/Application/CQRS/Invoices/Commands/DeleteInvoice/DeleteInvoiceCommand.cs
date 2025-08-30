// DeleteInvoiceCommand.cs
using MediatR;
using Application.Responses;

namespace Application.CQRS.Invoices.Commands.DeleteInvoice
{
    public class DeleteInvoiceCommand : IRequest<BaseCommandResponse>
    {
        public required int Id { get; set; }
    }
}