// Persistence/Repositories/InventoryAnalyticsRepository.cs
using Application.Contracts;
using Application.DTOs.Common;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class InventoryAnalyticsRepository : GenericRepository<InventoryAnalytics>, IInventoryAnalyticsRepository
    {
        public InventoryAnalyticsRepository(SIMSDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<PagedResult<InventoryAnalytics>> GetPagedAnalyticsAsync(int pageNumber, int pageSize, string? analysisPeriod, int? itemId, int? godownId, CancellationToken cancellationToken)
        {
            var query = _context.Set<InventoryAnalytics>().AsQueryable();

            if (!string.IsNullOrEmpty(analysisPeriod))
            {
                query = query.Where(a => a.AnalysisPeriod == analysisPeriod);
            }

            if (itemId.HasValue)
            {
                query = query.Where(a => a.ItemId == itemId.Value);
            }

            if (godownId.HasValue)
            {
                query = query.Where(a => a.GodownId == godownId.Value);
            }

            var totalCount = await query.CountAsync(cancellationToken);
            var items = await query
                .Include(a => a.Item)
                .Include(a => a.Godown)
                .OrderByDescending(a => a.AnalysisDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PagedResult<InventoryAnalytics>(items, totalCount, pageNumber, pageSize);
        }

        public async Task<IReadOnlyList<InventoryAnalytics>> GetAnalyticsByItemAsync(int itemId, string analysisPeriod, CancellationToken cancellationToken)
        {
            return await _context.Set<InventoryAnalytics>()
                .Where(a => a.ItemId == itemId && a.AnalysisPeriod == analysisPeriod)
                .Include(a => a.Item)
                .Include(a => a.Godown)
                .OrderByDescending(a => a.AnalysisDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<InventoryAnalytics>> GetABCAnalysisAsync(int godownId, CancellationToken cancellationToken)
        {
            return await _context.Set<InventoryAnalytics>()
                .Where(a => a.GodownId == godownId && !string.IsNullOrEmpty(a.ABCCategory))
                .Include(a => a.Item)
                .OrderBy(a => a.ABCCategory)
                .ThenByDescending(a => a.ABCValue)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<InventoryAnalytics>> GetXYZAnalysisAsync(int godownId, CancellationToken cancellationToken)
        {
            return await _context.Set<InventoryAnalytics>()
                .Where(a => a.GodownId == godownId && !string.IsNullOrEmpty(a.XYZCategory))
                .Include(a => a.Item)
                .OrderBy(a => a.XYZCategory)
                .ThenBy(a => a.DemandVariability)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<InventoryAnalytics>> GetSlowMovingItemsAsync(int godownId, int daysThreshold, CancellationToken cancellationToken)
        {
            return await _context.Set<InventoryAnalytics>()
                .Where(a => a.GodownId == godownId && a.DaysInStock > daysThreshold)
                .Include(a => a.Item)
                .OrderByDescending(a => a.DaysInStock)
                .ToListAsync(cancellationToken);
        }
    }
}