// Application/Contracts/IGodownInventoryRepository.cs
using Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface IGodownInventoryRepository : IGenericRepository<GodownInventory>
    {
        Task<GodownInventory?> GetByGodownIdAndItemIdAsync(int godownId, int itemId, CancellationToken cancellationToken);
    }
}