using Application.Contracts;
using Application.DTOs.Invoice;
using Application.Responses;
using Application.DTOs.Common;
using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.Invoices.Queries.GetInvoiceList
{
    public class GetInvoiceListQueryHandler(IInvoiceRepository invoiceRepository, IMapper mapper)
        : IRequestHandler<GetInvoiceListQuery, PagedResponse<InvoiceDto>>
    {
        public async Task<PagedResponse<InvoiceDto>> Handle(GetInvoiceListQuery request, CancellationToken cancellationToken)
        {
            // 💡 በገጽ የተከፋፈለ እና የተጣራ የሂሳብ መጠየቂያዎች ዝርዝር ያመጣል
            var pagedResult = await invoiceRepository.GetPagedInvoicesAsync(
                request.Parameters.PageNumber,
                request.Parameters.PageSize,
                request.Parameters.SearchTerm,
                cancellationToken);

            var invoiceDtos = mapper.Map<List<InvoiceDto>>(pagedResult.Items);

            return new PagedResponse<InvoiceDto>(
                invoiceDtos,
                pagedResult.TotalCount,
                pagedResult.PageNumber,
                pagedResult.PageSize);
        }
    }
}