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
    public class GodownInventoryRepository : GenericRepository<GodownInventory>, IGodownInventoryRepository
    {
        public GodownInventoryRepository(SIMSDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<GodownInventory?> GetByGodownAndItemAsync(int godownId, int itemId, CancellationToken cancellationToken)
        {
            return await _dbContext.GodownInventories
                .Include(x => x.Godown)
                .Include(x => x.Item)
                .FirstOrDefaultAsync(x => x.GodownId == godownId && x.ItemId == itemId, cancellationToken);
        }

        public async Task<IReadOnlyList<GodownInventory>> GetByGodownIdAsync(int godownId, CancellationToken cancellationToken)
        {
            return await _dbContext.GodownInventories
                .Where(x => x.GodownId == godownId)
                .Include(x => x.Item)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<GodownInventory>> GetLowStockItemsAsync(int threshold, CancellationToken cancellationToken)
        {
            return await _dbContext.GodownInventories
                .Where(x => x.Quantity <= threshold)
                .Include(x => x.Godown)
                .Include(x => x.Item)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> HasInventoryByGodownIdAsync(int godownId, CancellationToken cancellationToken)
        {
            return await _dbContext.GodownInventories.AnyAsync(x => x.GodownId == godownId, cancellationToken);
        }

        public async Task<bool> HasInventoryByItemIdAsync(int itemId, CancellationToken cancellationToken)
        {
            return await _dbContext.GodownInventories.AnyAsync(x => x.ItemId == itemId, cancellationToken);
        }
    }
}
