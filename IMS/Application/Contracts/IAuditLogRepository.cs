// Application/Contracts/IAuditLogRepository.cs
using Application.DTOs.Common;
using Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface IAuditLogRepository : IGenericRepository<AuditLog>
    {
        Task<PagedResult<AuditLog>> GetPagedAuditLogsAsync(int pageNumber, int pageSize, string? entityName, string? action, CancellationToken cancellationToken);
        Task<IReadOnlyList<AuditLog>> GetAuditLogsByEntityAsync(string entityName, string entityId, CancellationToken cancellationToken);
        Task<IReadOnlyList<AuditLog>> GetAuditLogsByUserAsync(string userId, DateTime? fromDate, DateTime? toDate, CancellationToken cancellationToken);
    }
}