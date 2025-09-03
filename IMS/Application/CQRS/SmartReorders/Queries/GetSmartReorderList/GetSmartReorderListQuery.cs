// Application/CQRS/SmartReorders/Queries/GetSmartReorderList/GetSmartReorderListQuery.cs
using Application.DTOs.Common;
using Application.DTOs.SmartReorder;
using MediatR;

namespace Application.CQRS.SmartReorders.Queries.GetSmartReorderList
{
    public class GetSmartReorderListQuery : IRequest<PagedResult<SmartReorderDto>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? Status { get; set; }
        public string? ReorderReason { get; set; }
    }
}