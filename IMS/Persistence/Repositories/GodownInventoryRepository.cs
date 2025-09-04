// Persistence/Repositories/GodownInventoryRepository.cs

using Application.Contracts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    // The class now implements the interface correctly.
    public class GodownInventoryRepository(SIMSDbContext dbContext) : IGodownInventoryRepository
    {
        public async Task<GodownInventory?> GetByGodownAndItemAsync(int godownId, int itemId, CancellationToken cancellationToken)
        {
            return await dbContext.GodownInventories
                                 .FirstOrDefaultAsync(gi => gi.GodownId == godownId && gi.ItemId == itemId, cancellationToken);
        }

        public async Task<IReadOnlyList<GodownInventory>> GetByGodownIdAsync(int godownId, CancellationToken cancellationToken)
        {
            return await dbContext.GodownInventories
                                 .Where(gi => gi.GodownId == godownId)
                                 .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<GodownInventory>> GetLowStockItemsAsync(int threshold, CancellationToken cancellationToken)
        {
            return await dbContext.GodownInventories
                                 .Where(gi => gi.Quantity < threshold)
                                 .ToListAsync(cancellationToken);
        }

        public async Task<bool> HasInventoryByGodownIdAsync(int id, CancellationToken cancellationToken)
        {
            return await dbContext.GodownInventories
                                 .AnyAsync(gi => gi.GodownId == id, cancellationToken);
        }

        public async Task<bool> HasInventoryByItemIdAsync(int id, CancellationToken cancellationToken)
        {
            return await dbContext.GodownInventories
                                 .AnyAsync(gi => gi.ItemId == id, cancellationToken);
        }

        // IGenericRepository<GodownInventory> members
        public async Task<IReadOnlyList<GodownInventory>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await dbContext.GodownInventories.ToListAsync(cancellationToken);
        }

        public async Task<GodownInventory?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await dbContext.GodownInventories.FindAsync(id, cancellationToken);
        }

        public async Task<GodownInventory> AddAsync(GodownInventory entity, CancellationToken cancellationToken)
        {
            await dbContext.GodownInventories.AddAsync(entity, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public async Task UpdateAsync(GodownInventory entity, CancellationToken cancellationToken)
        {
            dbContext.GodownInventories.Update(entity);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(GodownInventory entity, CancellationToken cancellationToken)
        {
            dbContext.GodownInventories.Remove(entity);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        public Task UpdateAsync(Notification notification, CancellationToken none)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<GodownInventory>> GetByGodownAsync(int godownId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<GodownInventory>> GetByItemAsync(int itemId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<GodownInventory>> GetLowStockItemsAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public void Update(GodownInventory entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(GodownInventory entity)
        {
            throw new NotImplementedException();
        }
    }
}