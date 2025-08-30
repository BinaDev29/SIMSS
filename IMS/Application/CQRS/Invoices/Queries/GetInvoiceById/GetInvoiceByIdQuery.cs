// GetInvoiceByIdQuery.cs
using MediatR;
using Application.DTOs.Invoice;

namespace Application.CQRS.Invoices.Queries.GetInvoiceById
{
    public class GetInvoiceByIdQuery : IRequest<InvoiceDto>
    {
        public required int Id { get; set; }
    }
}