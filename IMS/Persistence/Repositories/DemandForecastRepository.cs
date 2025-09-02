// Persistence/Repositories/DemandForecastRepository.cs
using Application.Contracts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class DemandForecastRepository : GenericRepository<DemandForecast>, IDemandForecastRepository
    {
        private readonly SIMSDbContext _context;

        public DemandForecastRepository(SIMSDbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

        public async Task<IReadOnlyList<DemandForecast>> GetForecastsByItemAsync(int itemId, CancellationToken cancellationToken)
        {
            return await _context.DemandForecasts
                .Include(df => df.Godown)
                .Where(df => df.ItemId == itemId)
                .OrderByDescending(df => df.ForecastDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<DemandForecast>> GetForecastsByGodownAsync(int godownId, CancellationToken cancellationToken)
        {
            return await _context.DemandForecasts
                .Include(df => df.Item)
                .Where(df => df.GodownId == godownId)
                .OrderByDescending(df => df.ForecastDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<DemandForecast>> GetForecastsByPeriodAsync(string period, CancellationToken cancellationToken)
        {
            return await _context.DemandForecasts
                .Include(df => df.Item)
                .Include(df => df.Godown)
                .Where(df => df.ForecastPeriod == period)
                .OrderByDescending(df => df.ForecastDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<DemandForecast?> GetLatestForecastAsync(int itemId, int godownId, CancellationToken cancellationToken)
        {
            return await _context.DemandForecasts
                .Include(df => df.Item)
                .Include(df => df.Godown)
                .Where(df => df.ItemId == itemId && df.GodownId == godownId)
                .OrderByDescending(df => df.ForecastDate)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<PagedResult<DemandForecast>> GetPagedForecastsAsync(int pageNumber, int pageSize, string? searchTerm, CancellationToken cancellationToken)
        {
            var query = _context.Set<DemandForecast>()
                .Include(df => df.Item)
                .Include(df => df.Godown)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(df => df.Item!.ItemName.Contains(searchTerm) || 
                                         df.ForecastPeriod.Contains(searchTerm) ||
                                         df.ForecastModel.Contains(searchTerm));
            }

            var totalCount = await query.CountAsync(cancellationToken);
            var items = await query
                .OrderByDescending(df => df.ForecastDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PagedResult<DemandForecast>(items, totalCount, pageNumber, pageSize);
        }

        Task<Application.DTOs.Common.PagedResult<DemandForecast>> IDemandForecastRepository.GetPagedForecastsAsync(int pageNumber, int pageSize, string? searchTerm, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}