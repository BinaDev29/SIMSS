// Persistence/Repositories/ItemRepository.cs
using Application.Contracts;
using Application.DTOs.Common;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class ItemRepository(SIMSDbContext dbContext) : GenericRepository<Item>(dbContext), IItemRepository
    {
        private new readonly SIMSDbContext _context = dbContext;

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
                query = query.Where(i => i.ItemName.Contains(searchTerm) || i.ItemCode.Contains(searchTerm));
            }

            var totalCount = await query.CountAsync(cancellationToken);
            var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);

            return new PagedResult<Item>(items, totalCount, pageNumber, pageSize);
        }
    }
}