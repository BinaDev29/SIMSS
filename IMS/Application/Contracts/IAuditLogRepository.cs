// Application/Contracts/IAuditLogRepository.cs
using Application.DTOs.Common;
using Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface IAuditLogRepository : IGenericRepository<AuditLog>
    {
        // Added the missing method definitions
        Task<PagedResult<AuditLog>> GetPagedAuditLogsAsync(
            int pageNumber,
            int pageSize,
            string? entityName,
            string? entityId,
            CancellationToken cancellationToken);

        Task<PagedResult<AuditLog>> GetPagedUserLogsAsync(
            int pageNumber,
            int pageSize,
            string? userId,
            CancellationToken cancellationToken);
    }
}