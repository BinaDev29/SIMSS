// Persistence/Repositories/AuditLogRepository.cs
using Application.Contracts;
using Application.DTOs.Common;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class AuditLogRepository : GenericRepository<AuditLog>, IAuditLogRepository
    {
        public AuditLogRepository(SIMSDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<PagedResult<AuditLog>> GetPagedAuditLogsAsync(int pageNumber, int pageSize, string? entityName, string? action, CancellationToken cancellationToken)
        {
            var query = _context.Set<AuditLog>().AsQueryable();

            if (!string.IsNullOrEmpty(entityName))
            {
                query = query.Where(a => a.EntityName == entityName);
            }

            if (!string.IsNullOrEmpty(action))
            {
                query = query.Where(a => a.Action == action);
            }

            var totalCount = await query.CountAsync(cancellationToken);
            var items = await query
                .OrderByDescending(a => a.Timestamp)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PagedResult<AuditLog>(items, totalCount, pageNumber, pageSize);
        }

        public async Task<IReadOnlyList<AuditLog>> GetAuditLogsByEntityAsync(string entityName, string entityId, CancellationToken cancellationToken)
        {
            return await _context.Set<AuditLog>()
                .Where(a => a.EntityName == entityName && a.EntityId == entityId)
                .OrderByDescending(a => a.Timestamp)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<AuditLog>> GetAuditLogsByUserAsync(string userId, DateTime? fromDate, DateTime? toDate, CancellationToken cancellationToken)
        {
            var query = _context.Set<AuditLog>().Where(a => a.UserId == userId);

            if (fromDate.HasValue)
            {
                query = query.Where(a => a.Timestamp >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                query = query.Where(a => a.Timestamp <= toDate.Value);
            }

            return await query
                .OrderByDescending(a => a.Timestamp)
                .ToListAsync(cancellationToken);
        }
    }
}