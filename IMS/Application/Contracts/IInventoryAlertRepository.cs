// Application/Contracts/IInventoryAlertRepository.cs
using Application.DTOs.Common;
using Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface IInventoryAlertRepository : IGenericRepository<InventoryAlert>
    {
        Task<IReadOnlyList<InventoryAlert>> GetActiveAlertsAsync(CancellationToken cancellationToken);
        Task<IReadOnlyList<InventoryAlert>> GetAlertsByTypeAsync(string alertType, CancellationToken cancellationToken);
        Task<IReadOnlyList<InventoryAlert>> GetAlertsByItemAsync(int itemId, CancellationToken cancellationToken);
        Task<PagedResult<InventoryAlert>> GetPagedAlertsAsync(int pageNumber, int pageSize, string? searchTerm, CancellationToken cancellationToken);
    }
}