using Application.Contracts;
using Application.DTOs.Invoice;
using AutoMapper;
using MediatR;
using Domain.Models;

namespace Application.CQRS.Invoices.Queries.GetInvoiceById
{
    public class GetInvoiceByIdQueryHandler(IInvoiceRepository repository, IMapper mapper) : IRequestHandler<GetInvoiceByIdQuery, InvoiceDto>
    {
        public async Task<InvoiceDto> Handle(GetInvoiceByIdQuery request, CancellationToken cancellationToken)
        {
            // Note: The repository should be configured to include InvoiceDetails when fetching the invoice.
            var invoice = await repository.GetByIdAsync(request.Id, cancellationToken);
            if (invoice is null)
                throw new KeyNotFoundException("Invoice not found");

            return mapper.Map<InvoiceDto>(invoice);
        }
    }
}