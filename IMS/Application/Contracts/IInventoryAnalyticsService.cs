using Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface IInventoryAnalyticsService : IGenericRepository<InventoryAnalytics>
    {
        Task<InventoryAnalytics> GenerateInventoryAnalyticsAsync(int itemId, int godownId, string analysisPeriod, CancellationToken cancellationToken);
        Task<IReadOnlyList<InventoryAnalytics>> GenerateBulkAnalyticsAsync(string analysisPeriod, CancellationToken cancellationToken);
        Task<IReadOnlyList<InventoryAnalytics>> GetABCAnalysisAsync(string category, CancellationToken cancellationToken);
        Task<IReadOnlyList<InventoryAnalytics>> GetVelocityAnalysisAsync(int itemId, CancellationToken cancellationToken);
        Task<decimal> CalculateTurnoverRatioAsync(int itemId, int godownId, CancellationToken cancellationToken);
        Task<decimal> CalculateStockoutRiskAsync(int itemId, int godownId, CancellationToken cancellationToken);
        Task<IReadOnlyList<InventoryAnalytics>> GetSlowMovingItemsAsync(int daysThreshold, CancellationToken cancellationToken);
        Task<IReadOnlyList<InventoryAnalytics>> GetFastMovingItemsAsync(int topCount, CancellationToken cancellationToken);
    }
}