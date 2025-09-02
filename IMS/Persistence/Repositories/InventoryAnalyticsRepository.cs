// Persistence/Repositories/InventoryAnalyticsRepository.cs
using Application.Contracts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class InventoryAnalyticsRepository : GenericRepository<InventoryAnalytics>, IInventoryAnalyticsRepository
    {
        private readonly SIMSDbContext _context;

        public InventoryAnalyticsRepository(SIMSDbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

        public async Task<IReadOnlyList<InventoryAnalytics>> GetAnalyticsByItemAsync(int itemId, CancellationToken cancellationToken)
        {
            return await _context.InventoryAnalytics
                .Include(ia => ia.Godown)
                .Where(ia => ia.ItemId == itemId)
                .OrderByDescending(ia => ia.AnalysisDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<InventoryAnalytics>> GetAnalyticsByGodownAsync(int godownId, CancellationToken cancellationToken)
        {
            return await _context.InventoryAnalytics
                .Include(ia => ia.Item)
                .Where(ia => ia.GodownId == godownId)
                .OrderByDescending(ia => ia.AnalysisDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<InventoryAnalytics>> GetAnalyticsByPeriodAsync(string period, CancellationToken cancellationToken)
        {
            return await _context.InventoryAnalytics
                .Include(ia => ia.Item)
                .Include(ia => ia.Godown)
                .Where(ia => ia.AnalysisPeriod == period)
                .OrderByDescending(ia => ia.AnalysisDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<InventoryAnalytics>> GetABCAnalysisAsync(string category, CancellationToken cancellationToken)
        {
            return await _context.InventoryAnalytics
                .Include(ia => ia.Item)
                .Include(ia => ia.Godown)
                .Where(ia => ia.ABCCategory == category)
                .OrderByDescending(ia => ia.ABCValue)
                .ToListAsync(cancellationToken);
        }

        public async Task<PagedResult<InventoryAnalytics>> GetPagedAnalyticsAsync(int pageNumber, int pageSize, string? searchTerm, CancellationToken cancellationToken)
        {
            var query = _context.Set<InventoryAnalytics>()
                .Include(ia => ia.Item)
                .Include(ia => ia.Godown)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(ia => ia.Item!.ItemName.Contains(searchTerm) || 
                                         ia.AnalysisPeriod.Contains(searchTerm) ||
                                         ia.ABCCategory!.Contains(searchTerm));
            }

            var totalCount = await query.CountAsync(cancellationToken);
            var items = await query
                .OrderByDescending(ia => ia.AnalysisDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PagedResult<InventoryAnalytics>(items, totalCount, pageNumber, pageSize);
        }

        Task<Application.DTOs.Common.PagedResult<InventoryAnalytics>> IInventoryAnalyticsRepository.GetPagedAnalyticsAsync(int pageNumber, int pageSize, string? searchTerm, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}