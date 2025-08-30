using Application.Contracts;
using Application.DTOs.Supplier;
using AutoMapper;
using MediatR;

namespace Application.CQRS.Suppliers.Queries.GetSupplierById
{
    public class GetSupplierByIdQueryHandler(ISupplierRepository supplierRepository, IMapper mapper)
        : IRequestHandler<GetSupplierByIdQuery, SupplierDto>
    {
        public async Task<SupplierDto> Handle(GetSupplierByIdQuery request, CancellationToken cancellationToken)
        {
            var supplier = await supplierRepository.GetByIdAsync(request.Id, cancellationToken);
            return mapper.Map<SupplierDto>(supplier);
        }
    }
}