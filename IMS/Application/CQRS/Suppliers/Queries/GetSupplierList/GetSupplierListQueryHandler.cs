using MediatR;
using Application.Contracts;
using Application.DTOs.Supplier;
using Application.Responses;
using Application.DTOs.Common;
using AutoMapper;

namespace Application.CQRS.Suppliers.Queries.GetSupplierList
{
    public class GetSupplierListQueryHandler(ISupplierRepository supplierRepository, IMapper mapper)
        : IRequestHandler<GetSupplierListQuery, PagedResponse<SupplierDto>>
    {
        public async Task<PagedResponse<SupplierDto>> Handle(GetSupplierListQuery request, CancellationToken cancellationToken)
        {
            // 💡 በገጽ የተከፋፈለ እና የተጣራ የአቅራቢዎች ዝርዝር ያመጣል
            var pagedResult = await supplierRepository.GetPagedSuppliersAsync(
                request.Parameters.PageNumber,
                request.Parameters.PageSize,
                request.Parameters.SearchTerm,
                cancellationToken);

            var supplierDtos = mapper.Map<List<SupplierDto>>(pagedResult.Items);

            return new PagedResponse<SupplierDto>(
                supplierDtos,
                pagedResult.TotalCount,
                pagedResult.PageNumber,
                pagedResult.PageSize);
        }
    }
}