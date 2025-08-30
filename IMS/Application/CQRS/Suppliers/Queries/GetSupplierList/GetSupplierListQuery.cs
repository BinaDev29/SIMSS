// GetSupplierListQuery.cs
using Application.DTOs.Supplier;
using MediatR;
using System.Collections.Generic;

namespace Application.CQRS.Suppliers.Queries.GetSupplierList
{
    public class GetSupplierListQuery : IRequest<List<SupplierDto>>
    {
    }
}