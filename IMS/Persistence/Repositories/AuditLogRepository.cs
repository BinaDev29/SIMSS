// Persistence/Repositories/AuditLogRepository.cs
using Application.Contracts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class AuditLogRepository : GenericRepository<AuditLog>, IAuditLogRepository
    {
        private readonly SIMSDbContext _context;

        public AuditLogRepository(SIMSDbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

        public async Task<IReadOnlyList<AuditLog>> GetLogsByEntityAsync(string entityName, string entityId, CancellationToken cancellationToken)
        {
            return await _context.AuditLogs
                .Where(al => al.EntityName == entityName && al.EntityId == entityId)
                .OrderByDescending(al => al.Timestamp)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<AuditLog>> GetLogsByUserAsync(string userId, CancellationToken cancellationToken)
        {
            return await _context.AuditLogs
                .Where(al => al.UserId == userId)
                .OrderByDescending(al => al.Timestamp)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<AuditLog>> GetLogsByActionAsync(string action, CancellationToken cancellationToken)
        {
            return await _context.AuditLogs
                .Where(al => al.Action == action)
                .OrderByDescending(al => al.Timestamp)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<AuditLog>> GetLogsByDateRangeAsync(DateTime fromDate, DateTime toDate, CancellationToken cancellationToken)
        {
            return await _context.AuditLogs
                .Where(al => al.Timestamp >= fromDate && al.Timestamp <= toDate)
                .OrderByDescending(al => al.Timestamp)
                .ToListAsync(cancellationToken);
        }

        public async Task<PagedResult<AuditLog>> GetPagedLogsAsync(int pageNumber, int pageSize, string? searchTerm, CancellationToken cancellationToken)
        {
            var query = _context.Set<AuditLog>().AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(al => al.EntityName.Contains(searchTerm) || 
                                         al.Action.Contains(searchTerm) ||
                                         al.UserName.Contains(searchTerm) ||
                                         al.Details.Contains(searchTerm));
            }

            var totalCount = await query.CountAsync(cancellationToken);
            var items = await query
                .OrderByDescending(al => al.Timestamp)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PagedResult<AuditLog>(items, totalCount, pageNumber, pageSize);
        }

        Task<Application.DTOs.Common.PagedResult<AuditLog>> IAuditLogRepository.GetPagedLogsAsync(int pageNumber, int pageSize, string? searchTerm, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> Query()
        {
            throw new NotImplementedException();
        }
    }
}