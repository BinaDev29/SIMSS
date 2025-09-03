// Application/Contracts/IAuditLogRepository.cs
using Application.DTOs.Common;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface IAuditLogRepository : IGenericRepository<AuditLog>
    {
        Task<IReadOnlyList<AuditLog>> GetLogsByEntityAsync(string entityName, string entityId, CancellationToken cancellationToken);
        Task<IReadOnlyList<AuditLog>> GetLogsByUserAsync(string userId, CancellationToken cancellationToken);
        Task<IReadOnlyList<AuditLog>> GetLogsByActionAsync(string action, CancellationToken cancellationToken);
        Task<IReadOnlyList<AuditLog>> GetLogsByDateRangeAsync(DateTime fromDate, DateTime toDate, CancellationToken cancellationToken);
        Task<PagedResult<AuditLog>> GetPagedLogsAsync(int pageNumber, int pageSize, string? searchTerm, CancellationToken cancellationToken);
        IEnumerable<object> Query();
    }
}