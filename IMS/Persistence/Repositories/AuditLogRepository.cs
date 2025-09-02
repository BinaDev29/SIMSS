// Persistence/Repositories/AuditLogRepository.cs
using Application.Contracts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<IReadOnlyList<AuditLog>> GetAllLogsAsync(CancellationToken cancellationToken)
        {
            return await _context.AuditLogs
                .OrderByDescending(a => a.Timestamp)
                .ToListAsync(cancellationToken);
        }

        public async Task<AuditLog?> GetLogByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.AuditLogs.FindAsync(new object[] { id }, cancellationToken);
        }

        public async Task AddLogAsync(AuditLog log, CancellationToken cancellationToken)
        {
            await _context.AuditLogs.AddAsync(log, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateLogAsync(AuditLog log, CancellationToken cancellationToken)
        {
            _context.Entry(log).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteLogAsync(AuditLog log, CancellationToken cancellationToken)
        {
            _context.AuditLogs.Remove(log);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
