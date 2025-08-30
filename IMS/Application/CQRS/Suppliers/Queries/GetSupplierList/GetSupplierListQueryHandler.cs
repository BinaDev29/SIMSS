using Application.Contracts;
using Application.DTOs.Supplier;
using AutoMapper;
using MediatR;
using System.Collections.Generic;

namespace Application.CQRS.Suppliers.Queries.GetSupplierList
{
    public class GetSupplierListQueryHandler(ISupplierRepository supplierRepository, IMapper mapper)
        : IRequestHandler<GetSupplierListQuery, List<SupplierDto>>
    {
        public async Task<List<SupplierDto>> Handle(GetSupplierListQuery request, CancellationToken cancellationToken)
        {
            var suppliers = await supplierRepository.GetAllAsync(cancellationToken);
            return mapper.Map<List<SupplierDto>>(suppliers);
        }
    }
}