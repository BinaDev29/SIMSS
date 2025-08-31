// Application/Contracts/IInventoryAlertRepository.cs
using Application.DTOs.Common;
using Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface IInventoryAlertRepository : IGenericRepository<InventoryAlert>
    {
        Task<PagedResult<InventoryAlert>> GetPagedInventoryAlertsAsync(int pageNumber, int pageSize, string? alertType, string? severity, bool? isActive, CancellationToken cancellationToken);
        Task<IReadOnlyList<InventoryAlert>> GetActiveAlertsByItemAsync(int itemId, CancellationToken cancellationToken);
        Task<IReadOnlyList<InventoryAlert>> GetActiveAlertsByGodownAsync(int godownId, CancellationToken cancellationToken);
        Task<IReadOnlyList<InventoryAlert>> GetCriticalAlertsAsync(CancellationToken cancellationToken);
        Task AcknowledgeAlertAsync(int alertId, string acknowledgedBy, CancellationToken cancellationToken);
        Task AddAsync(InventoryAlert inventoryAlert);
    }
}