// Application/Contracts/IDemandForecastRepository.cs
using Application.DTOs.Common;
using Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface IDemandForecastRepository : IGenericRepository<DemandForecast>
    {
        Task<PagedResult<DemandForecast>> GetPagedForecastsAsync(int pageNumber, int pageSize, string? forecastPeriod, int? itemId, int? godownId, CancellationToken cancellationToken);
        Task<IReadOnlyList<DemandForecast>> GetForecastsByItemAsync(int itemId, string forecastPeriod, CancellationToken cancellationToken);
        Task<DemandForecast?> GetLatestForecastAsync(int itemId, int godownId, string forecastPeriod, CancellationToken cancellationToken);
        Task<IReadOnlyList<DemandForecast>> GetForecastsForAccuracyCheckAsync(DateTime cutoffDate, CancellationToken cancellationToken);
        Task UpdateActualDemandAsync(int forecastId, decimal actualDemand, CancellationToken cancellationToken);
        Task<decimal> GetAverageAccuracyByModelAsync(string forecastModel, CancellationToken cancellationToken);
    }
}