using MediatR;
using Application.DTOs.Invoice;

namespace Application.CQRS.InvoiceDetails.Queries.GetInvoiceDetailById
{
    public class GetInvoiceDetailByIdQuery : IRequest<InvoiceDetailDto>
    {
        public required int Id { get; set; }
    }
}