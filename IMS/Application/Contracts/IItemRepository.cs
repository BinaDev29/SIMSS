// Application/Contracts/IItemRepository.cs
using Application.DTOs.Common;
using Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface IItemRepository : IGenericRepository<Item>
    {
        Task<Item?> GetItemByNameOrCodeAsync(string name, string code, CancellationToken cancellationToken);
        Task<bool> HasInventoryByItemIdAsync(int itemId, CancellationToken cancellationToken);
        Task<bool> HasTransactionsByItemIdAsync(int itemId, CancellationToken cancellationToken);
        Task<bool> HasInvoiceDetailsByItemIdAsync(int itemId, CancellationToken cancellationToken);
        Task<PagedResult<Item>> GetPagedItemsAsync(int pageNumber, int pageSize, string? searchTerm, CancellationToken cancellationToken);
    }
}