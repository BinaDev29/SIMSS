using MediatR;
using Application.Contracts;
using Application.DTOs.Invoice;
using AutoMapper;

namespace Application.CQRS.InvoiceDetails.Queries.GetInvoiceDetailList
{
    public class GetInvoiceDetailListQueryHandler(IInvoiceDetailRepository invoiceDetailRepository, IMapper mapper) : IRequestHandler<GetInvoiceDetailListQuery, List<InvoiceDetailDto>>
    {
        public async Task<List<InvoiceDetailDto>> Handle(GetInvoiceDetailListQuery request, CancellationToken cancellationToken)
        {
            var invoiceDetails = await invoiceDetailRepository.GetAllAsync(cancellationToken);
            
            // Apply search filter if provided
            if (!string.IsNullOrEmpty(request.QueryParameters?.SearchTerm))
            {
                var searchTerm = request.QueryParameters.SearchTerm.ToLower();
                invoiceDetails = invoiceDetails.Where(x => 
                    x.Item?.ItemName.ToLower().Contains(searchTerm) == true ||
                    x.Invoice?.InvoiceNumber?.ToLower().Contains(searchTerm) == true).ToList();
            }

            return mapper.Map<List<InvoiceDetailDto>>(invoiceDetails);
        }
    }
}