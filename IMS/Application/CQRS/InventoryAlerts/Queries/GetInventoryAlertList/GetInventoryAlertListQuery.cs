// Application/CQRS/InventoryAlerts/Queries/GetInventoryAlertList/GetInventoryAlertListQuery.cs
using Application.DTOs.Common;
using Application.DTOs.InventoryAlert;
using MediatR;

namespace Application.CQRS.InventoryAlerts.Queries.GetInventoryAlertList
{
    public class GetInventoryAlertListQuery : IRequest<PagedResult<InventoryAlertDto>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? AlertType { get; set; }
        public string? Severity { get; set; }
        public bool? IsActive { get; set; }
    }
}