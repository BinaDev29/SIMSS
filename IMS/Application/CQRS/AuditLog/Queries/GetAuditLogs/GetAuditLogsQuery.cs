using Application.DTOs.AuditLog;
using MediatR;

namespace Application.CQRS.AuditLog.Queries.GetAuditLogs
{
    public class GetAuditLogsQuery : IRequest<IReadOnlyList<AuditLogDto>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? EntityName { get; set; }
        public string? UserId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string? Action { get; set; }
        public string? SortBy { get; set; }
        public bool SortDescending { get; set; } = true;
    }
}