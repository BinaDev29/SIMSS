using MediatR;
using Application.DTOs.Invoice;
using System.Collections.Generic;

namespace Application.CQRS.InvoiceDetails.Queries.GetInvoiceDetailList
{
    public class GetInvoiceDetailListQuery : IRequest<List<InvoiceDetailDto>>
    {
    }
}