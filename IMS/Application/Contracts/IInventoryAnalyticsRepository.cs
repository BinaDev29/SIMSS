// Application/Contracts/IInventoryAnalyticsRepository.cs
using Application.DTOs.Common;
using Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface IInventoryAnalyticsRepository : IGenericRepository<InventoryAnalytics>
    {
        Task<PagedResult<InventoryAnalytics>> GetPagedAnalyticsAsync(int pageNumber, int pageSize, string? analysisPeriod, int? itemId, int? godownId, CancellationToken cancellationToken);
        Task<IReadOnlyList<InventoryAnalytics>> GetAnalyticsByItemAsync(int itemId, string analysisPeriod, CancellationToken cancellationToken);
        Task<IReadOnlyList<InventoryAnalytics>> GetABCAnalysisAsync(int godownId, CancellationToken cancellationToken);
        Task<IReadOnlyList<InventoryAnalytics>> GetXYZAnalysisAsync(int godownId, CancellationToken cancellationToken);
        Task<IReadOnlyList<InventoryAnalytics>> GetSlowMovingItemsAsync(int godownId, int daysThreshold, CancellationToken cancellationToken);
    }
}