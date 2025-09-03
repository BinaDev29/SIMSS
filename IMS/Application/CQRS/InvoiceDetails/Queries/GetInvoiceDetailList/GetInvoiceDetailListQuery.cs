using MediatR;
using Application.DTOs.Invoice;
using Application.DTOs.Common;

namespace Application.CQRS.InvoiceDetails.Queries.GetInvoiceDetailList
{
    public class GetInvoiceDetailListQuery : IRequest<List<InvoiceDetailDto>>
    {
        public DTOs.Common.InvoiceDetailQueryParameters? QueryParameters { get; set; }
    }
}