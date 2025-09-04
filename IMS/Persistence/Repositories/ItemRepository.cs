// Persistence/Repositories/ItemRepository.cs
using Application.Contracts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class ItemRepository : GenericRepository<Item>, IItemRepository
    {
        private readonly SIMSDbContext _context;

        public ItemRepository(SIMSDbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

        public async Task<Item?> GetItemByNameOrCodeAsync(string name, string code, CancellationToken cancellationToken)
        {
            return await _context.Items.FirstOrDefaultAsync(i => i.ItemName == name || i.ItemCode == code, cancellationToken);
        }

        public async Task<bool> HasInventoryByItemIdAsync(int itemId, CancellationToken cancellationToken)
        {
            return await _context.GodownInventories.AnyAsync(gi => gi.ItemId == itemId, cancellationToken);
        }

        public async Task<bool> HasTransactionsByItemIdAsync(int itemId, CancellationToken cancellationToken)
        {
            return await _context.OutwardTransactions.AnyAsync(ot => ot.ItemId == itemId, cancellationToken);
        }

        public async Task<bool> HasInvoiceDetailsByItemIdAsync(int itemId, CancellationToken cancellationToken)
        {
            return await _context.InvoiceDetails.AnyAsync(id => id.ItemId == itemId, cancellationToken);
        }

        public async Task<PagedResult<Item>> GetPagedItemsAsync(int pageNumber, int pageSize, string? searchTerm, CancellationToken cancellationToken)
        {
            var query = _context.Set<Item>().AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(i => i.ItemName.Contains(searchTerm) || 
                                        i.ItemCode.Contains(searchTerm) ||
                                        i.Category!.Contains(searchTerm));
            }

            var totalCount = await query.CountAsync(cancellationToken);
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PagedResult<Item>(items, totalCount, pageNumber, pageSize);
        }

        public async Task<IReadOnlyList<Item>> GetLowStockItemsAsync(CancellationToken cancellationToken)
        {
            return await _context.Items
                .Where(i => i.StockQuantity <= i.ReorderLevel)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<Item>> GetItemsByCategoryAsync(string category, CancellationToken cancellationToken)
        {
            return await _context.Items
                .Where(i => i.Category == category)
                .ToListAsync(cancellationToken);
        }

        Task<Application.DTOs.Common.PagedResult<Item>> IItemRepository.GetPagedItemsAsync(int pageNumber, int pageSize, string? searchTerm, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Item> GetByIdAsync(object itemId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}