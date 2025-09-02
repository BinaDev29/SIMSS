// Persistence/Repositories/DemandForecastRepository.cs
using Application.Contracts;
using Application.DTOs.Common;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System;

namespace Persistence.Repositories
{
    public class DemandForecastRepository : GenericRepository<DemandForecast>, IDemandForecastRepository
    {
        public DemandForecastRepository(SIMSDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<PagedResult<DemandForecast>> GetPagedForecastsAsync(int pageNumber, int pageSize, string? forecastPeriod, int? itemId, int? godownId, CancellationToken cancellationToken)
        {
            var query = _dbContext.Set<DemandForecast>().AsQueryable();

            if (!string.IsNullOrEmpty(forecastPeriod))
            {
                query = query.Where(f => f.ForecastPeriod == forecastPeriod);
            }

            if (itemId.HasValue)
            {
                query = query.Where(f => f.ItemId == itemId.Value);
            }

            if (godownId.HasValue)
            {
                query = query.Where(f => f.GodownId == godownId.Value);
            }

            var totalCount = await query.CountAsync(cancellationToken);
            var items = await query
                .Include(f => f.Item)
                .Include(f => f.Godown)
                .OrderByDescending(f => f.ForecastDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PagedResult<DemandForecast>(items, totalCount, pageNumber, pageSize);
        }

        public async Task<IReadOnlyList<DemandForecast>> GetForecastsByItemAsync(int itemId, string forecastPeriod, CancellationToken cancellationToken)
        {
            return await _dbContext.Set<DemandForecast>()
                .Where(f => f.ItemId == itemId && f.ForecastPeriod == forecastPeriod)
                .Include(f => f.Item)
                .Include(f => f.Godown)
                .OrderByDescending(f => f.ForecastDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<DemandForecast?> GetLatestForecastAsync(int itemId, int godownId, string forecastPeriod, CancellationToken cancellationToken)
        {
            return await _dbContext.Set<DemandForecast>()
                .Where(f => f.ItemId == itemId && f.GodownId == godownId && f.ForecastPeriod == forecastPeriod)
                .Include(f => f.Item)
                .Include(f => f.Godown)
                .OrderByDescending(f => f.ForecastDate)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<DemandForecast>> GetForecastsForAccuracyCheckAsync(DateTime cutoffDate, CancellationToken cancellationToken)
        {
            return await _dbContext.Set<DemandForecast>()
                .Where(f => f.ForecastDate <= cutoffDate && f.ActualDemand == null)
                .Include(f => f.Item)
                .Include(f => f.Godown)
                .ToListAsync(cancellationToken);
        }

        public async Task UpdateActualDemandAsync(int forecastId, decimal actualDemand, CancellationToken cancellationToken)
        {
            var forecast = await _dbContext.Set<DemandForecast>().FindAsync(new object[] { forecastId }, cancellationToken);
            if (forecast != null)
            {
                forecast.ActualDemand = actualDemand;
                forecast.ActualDate = DateTime.UtcNow;
                forecast.ForecastError = Math.Abs(forecast.PredictedDemand - actualDemand);
                // ????? ?? ???? ?????
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task<decimal> GetAverageAccuracyByModelAsync(string forecastModel, CancellationToken cancellationToken)
        {
            var forecasts = await _dbContext.Set<DemandForecast>()
                .Where(f => f.ForecastModel == forecastModel && f.ActualDemand.HasValue)
                .ToListAsync(cancellationToken);

            if (!forecasts.Any())
                return 0;

            // ? accuracyScore? ????
            return forecasts.Average(f => f.AccuracyScore);
        }
    }
}