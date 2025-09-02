// Application/Contracts/IDemandForecastRepository.cs
using Application.DTOs.Common;
using Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface IDemandForecastRepository : IGenericRepository<DemandForecast>
    {
        Task<IReadOnlyList<DemandForecast>> GetForecastsByItemAsync(int itemId, CancellationToken cancellationToken);
        Task<IReadOnlyList<DemandForecast>> GetForecastsByGodownAsync(int godownId, CancellationToken cancellationToken);
        Task<IReadOnlyList<DemandForecast>> GetForecastsByPeriodAsync(string period, CancellationToken cancellationToken);
        Task<DemandForecast?> GetLatestForecastAsync(int itemId, int godownId, CancellationToken cancellationToken);
        Task<PagedResult<DemandForecast>> GetPagedForecastsAsync(int pageNumber, int pageSize, string? searchTerm, CancellationToken cancellationToken);
    }
}