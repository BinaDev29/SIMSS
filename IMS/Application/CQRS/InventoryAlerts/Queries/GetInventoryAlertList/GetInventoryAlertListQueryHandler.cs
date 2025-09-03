// Application/CQRS/InventoryAlerts/Queries/GetInventoryAlertList/GetInventoryAlertListQueryHandler.cs
using Application.Contracts;
using Application.DTOs.Common;
using Application.DTOs.InventoryAlert;
using AutoMapper;
using MediatR;

namespace Application.CQRS.InventoryAlerts.Queries.GetInventoryAlertList
{
    public class GetInventoryAlertListQueryHandler : IRequestHandler<GetInventoryAlertListQuery, PagedResult<InventoryAlertDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetInventoryAlertListQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PagedResult<InventoryAlertDto>> Handle(GetInventoryAlertListQuery request, CancellationToken cancellationToken)
        {
            var pagedAlerts = await _unitOfWork.InventoryAlertRepository.GetPagedInventoryAlertsAsync(
                request.PageNumber, 
                request.PageSize, 
                request.AlertType, 
                request.Severity, 
                request.IsActive, 
                cancellationToken);

            var alertDtos = _mapper.Map<List<InventoryAlertDto>>(pagedAlerts.Items);
            
            return new PagedResult<InventoryAlertDto>(alertDtos, pagedAlerts.TotalCount, pagedAlerts.PageNumber, pagedAlerts.PageSize);
        }
    }
}