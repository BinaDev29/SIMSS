// Application/CQRS/SmartReorders/Queries/GetSmartReorderList/GetSmartReorderListQueryHandler.cs
using Application.Contracts;
using Application.DTOs.Common;
using Application.DTOs.SmartReorder;
using AutoMapper;
using MediatR;

namespace Application.CQRS.SmartReorders.Queries.GetSmartReorderList
{
    public class GetSmartReorderListQueryHandler : IRequestHandler<GetSmartReorderListQuery, PagedResult<SmartReorderDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetSmartReorderListQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PagedResult<SmartReorderDto>> Handle(GetSmartReorderListQuery request, CancellationToken cancellationToken)
        {
            var pagedReorders = await _unitOfWork.SmartReorderRepository.GetPagedReordersAsync(
                request.PageNumber, 
                request.PageSize, 
                request.Status, 
                request.ReorderReason, 
                cancellationToken);

            var reorderDtos = _mapper.Map<List<SmartReorderDto>>(pagedReorders.Items);
            
            return new PagedResult<SmartReorderDto>(reorderDtos, pagedReorders.TotalCount, pagedReorders.PageNumber, pagedReorders.PageSize);
        }
    }
}