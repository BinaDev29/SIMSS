using MediatR;
using Application.DTOs.Supplier;
using Application.DTOs.Common;
using Application.Responses;

namespace Application.CQRS.Suppliers.Queries.GetSupplierList
{
    public class GetSupplierListQuery : IRequest<PagedResponse<SupplierDto>>
    {
        public required SupplierQueryParameters Parameters { get; set; }
    }
}