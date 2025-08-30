// Persistence/Repositories/GodownInventoryRepository.cs
using Application.Contracts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class GodownInventoryRepository(SIMSDbContext dbContext) : GenericRepository<GodownInventory>(dbContext), IGodownInventoryRepository
    {
        public async Task<GodownInventory?> GetByGodownIdAndItemIdAsync(int godownId, int itemId, CancellationToken cancellationToken)
        {
            return await DbContext.GodownInventories
                .FirstOrDefaultAsync(gi => gi.GodownId == godownId && gi.ItemId == itemId, cancellationToken);
        }
    }
}