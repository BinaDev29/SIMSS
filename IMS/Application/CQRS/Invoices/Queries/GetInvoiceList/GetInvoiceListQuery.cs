using MediatR;
using Application.DTOs.Invoice;
using Application.DTOs.Common;
using Application.Responses;

namespace Application.CQRS.Invoices.Queries.GetInvoiceList
{
    public class GetInvoiceListQuery : IRequest<PagedResponse<InvoiceDto>>
    {
        public required InvoiceQueryParameters Parameters { get; set; }
    }
}