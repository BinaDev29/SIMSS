using MediatR;
using Application.Contracts;
using Application.DTOs.Invoice;
using AutoMapper;

namespace Application.CQRS.InvoiceDetails.Queries.GetInvoiceDetailById
{
    public class GetInvoiceDetailByIdQueryHandler(IInvoiceDetailRepository invoiceDetailRepository, IMapper mapper)
        : IRequestHandler<GetInvoiceDetailByIdQuery, InvoiceDetailDto>
    {
        public async Task<InvoiceDetailDto> Handle(GetInvoiceDetailByIdQuery request, CancellationToken cancellationToken)
        {
            var invoiceDetail = await invoiceDetailRepository.GetByIdAsync(request.Id, cancellationToken);
            return mapper.Map<InvoiceDetailDto>(invoiceDetail);
        }
    }
}