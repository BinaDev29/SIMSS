// Persistence/Repositories/InventoryReportRepository.cs
using Application.Contracts;
using Application.DTOs.Common;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class InventoryReportRepository : GenericRepository<InventoryReport>, IInventoryReportRepository
    {
        public InventoryReportRepository(SIMSDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<PagedResult<InventoryReport>> GetPagedReportsAsync(int pageNumber, int pageSize, string? reportType, CancellationToken cancellationToken)
        {
            var query = _context.Set<InventoryReport>().AsQueryable();

            if (!string.IsNullOrEmpty(reportType))
            {
                query = query.Where(r => r.ReportType == reportType);
            }

            var totalCount = await query.CountAsync(cancellationToken);
            var items = await query
                .OrderByDescending(r => r.GeneratedDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PagedResult<InventoryReport>(items, totalCount, pageNumber, pageSize);
        }

        public async Task<IReadOnlyList<InventoryReport>> GetReportsByTypeAsync(string reportType, CancellationToken cancellationToken)
        {
            return await _context.Set<InventoryReport>()
                .Where(r => r.ReportType == reportType)
                .OrderByDescending(r => r.GeneratedDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<InventoryReport>> GetReportsByStatusAsync(string status, CancellationToken cancellationToken)
        {
            return await _context.Set<InventoryReport>()
                .Where(r => r.Status == status)
                .OrderByDescending(r => r.GeneratedDate)
                .ToListAsync(cancellationToken);
        }
    }
}