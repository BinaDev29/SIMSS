// Application/Contracts/IInventoryAnalyticsRepository.cs
using Application.DTOs.Common;
using Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface IInventoryAnalyticsRepository : IGenericRepository<InventoryAnalytics>
    {
        Task<IReadOnlyList<InventoryAnalytics>> GetAnalyticsByItemAsync(int itemId, CancellationToken cancellationToken);
        Task<IReadOnlyList<InventoryAnalytics>> GetAnalyticsByGodownAsync(int godownId, CancellationToken cancellationToken);
        Task<IReadOnlyList<InventoryAnalytics>> GetAnalyticsByPeriodAsync(string period, CancellationToken cancellationToken);
        Task<IReadOnlyList<InventoryAnalytics>> GetABCAnalysisAsync(string category, CancellationToken cancellationToken);
        Task<PagedResult<InventoryAnalytics>> GetPagedAnalyticsAsync(int pageNumber, int pageSize, string? searchTerm, CancellationToken cancellationToken);
    }
}