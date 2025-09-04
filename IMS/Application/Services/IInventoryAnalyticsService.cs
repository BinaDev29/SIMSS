// Application/Services/IInventoryAnalyticsService.cs
using System.Threading;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface IInventoryAnalyticsService
    {
        Task<object> GetInventoryAnalyticsAsync(CancellationToken cancellationToken = default);
        Task<object> GetLowStockItemsAsync(CancellationToken cancellationToken = default);
        Task<object> GetInventoryTrendsAsync(int days, CancellationToken cancellationToken = default);
    }
}