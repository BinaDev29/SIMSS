// Application/Contracts/IGodownInventoryRepository.cs

using Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface IGodownInventoryRepository : IGenericRepository<GodownInventory>
    {
        Task<GodownInventory?> GetByGodownAndItemAsync(int godownId, int itemId, CancellationToken cancellationToken);
        Task<IReadOnlyList<GodownInventory>> GetByGodownIdAsync(int godownId, CancellationToken cancellationToken);
        Task<IReadOnlyList<GodownInventory>> GetLowStockItemsAsync(int threshold, CancellationToken cancellationToken);
        Task<bool> HasInventoryByGodownIdAsync(int id, CancellationToken cancellationToken);
        Task<bool> HasInventoryByItemIdAsync(int id, CancellationToken cancellationToken);
    }
}