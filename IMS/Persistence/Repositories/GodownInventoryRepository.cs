// Persistence/Repositories/GodownInventoryRepository.cs
using Application.Contracts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class GodownInventoryRepository(SIMSDbContext context) : GenericRepository<GodownInventory>(context), IGodownInventoryRepository
    {
        private new readonly SIMSDbContext _context = context;

        public async Task<GodownInventory?> GetByGodownAndItemAsync(int godownId, int itemId, CancellationToken cancellationToken)
        {
            return await _context.GodownInventories
                .Include(x => x.Godown)
                .Include(x => x.Item)
                .FirstOrDefaultAsync(x => x.GodownId == godownId && x.ItemId == itemId, cancellationToken);
        }

        public async Task<IReadOnlyList<GodownInventory>> GetByGodownIdAsync(int godownId, CancellationToken cancellationToken)
        {
            return await _context.GodownInventories
                .Where(x => x.GodownId == godownId)
                .Include(x => x.Item)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<GodownInventory>> GetLowStockItemsAsync(int threshold, CancellationToken cancellationToken)
        {
            return await _context.GodownInventories
                .Where(x => x.Quantity <= threshold)
                .Include(x => x.Godown)
                .Include(x => x.Item)
                .ToListAsync(cancellationToken);
        }

        public Task<bool> HasInventoryByGodownIdAsync(int id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> HasInventoryByItemIdAsync(int id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}