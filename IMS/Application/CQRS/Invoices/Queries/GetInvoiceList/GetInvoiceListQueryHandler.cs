using Application.Contracts;
using Application.DTOs.Invoice;
using AutoMapper;
using MediatR;
using System.Collections.Generic;
using Domain.Models;

namespace Application.CQRS.Invoices.Queries.GetInvoiceList
{
    public class GetInvoiceListQueryHandler(IInvoiceRepository repository, IMapper mapper) : IRequestHandler<GetInvoiceListQuery, IEnumerable<InvoiceDto>>
    {
        public async Task<IEnumerable<InvoiceDto>> Handle(GetInvoiceListQuery request, CancellationToken cancellationToken)
        {
            var invoices = await repository.GetAllAsync(cancellationToken);
            return mapper.Map<IEnumerable<InvoiceDto>>(invoices);
        }
    }
}