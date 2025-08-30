using MediatR;
using Application.Contracts;
using Application.DTOs.Invoice;
using AutoMapper;
using System.Collections.Generic;

namespace Application.CQRS.InvoiceDetails.Queries.GetInvoiceDetailList
{
    public class GetInvoiceDetailListQueryHandler(IInvoiceDetailRepository invoiceDetailRepository, IMapper mapper)
        : IRequestHandler<GetInvoiceDetailListQuery, List<InvoiceDetailDto>>
    {
        public async Task<List<InvoiceDetailDto>> Handle(GetInvoiceDetailListQuery request, CancellationToken cancellationToken)
        {
            var invoiceDetails = await invoiceDetailRepository.GetAllAsync(cancellationToken);
            return mapper.Map<List<InvoiceDetailDto>>(invoiceDetails);
        }
    }
}