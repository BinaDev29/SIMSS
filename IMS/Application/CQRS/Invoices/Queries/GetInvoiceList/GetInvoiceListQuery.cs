// GetInvoiceListQuery.cs
using MediatR;
using Application.DTOs.Invoice;
using System.Collections.Generic;

namespace Application.CQRS.Invoices.Queries.GetInvoiceList
{
    public class GetInvoiceListQuery : IRequest<List<InvoiceDto>>
    {
    }
}