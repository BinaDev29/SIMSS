using Application.DTOs.Audit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface IAuditService
    {
        Task LogAsync(string entityName, string entityId, string action, object? oldValues, object? newValues, string userId, string userName);
        Task<IEnumerable<AuditLogDto>> GetAuditLogsAsync(string? entityName = null, string? entityId = null, int pageNumber = 1, int pageSize = 50);
        Task<IEnumerable<AuditLogDto>> GetUserActivityAsync(string userId, int pageNumber = 1, int pageSize = 50);
        Task<AuditSummaryDto> GetAuditSummaryAsync(DateTime fromDate, DateTime toDate);
    }
}