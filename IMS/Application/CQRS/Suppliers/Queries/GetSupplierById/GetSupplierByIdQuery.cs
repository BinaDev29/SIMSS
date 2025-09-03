// GetSupplierByIdQuery.cs
using Application.DTOs.Supplier;
using MediatR;

namespace Application.CQRS.Suppliers.Queries.GetSupplierById
{
    public class GetSupplierByIdQuery : IRequest<SupplierDto>
    {
        public required int Id { get; set; }
    }
}