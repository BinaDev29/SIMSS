// Persistence/Repositories/InventoryAlertRepository.cs
using Application.Contracts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class InventoryAlertRepository : GenericRepository<InventoryAlert>, IInventoryAlertRepository
    {
        private readonly SIMSDbContext _context;

        public InventoryAlertRepository(SIMSDbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

        public async Task<IReadOnlyList<InventoryAlert>> GetActiveAlertsAsync(CancellationToken cancellationToken)
        {
            return await _context.InventoryAlerts
                .Include(ia => ia.Item)
                .Include(ia => ia.Godown)
                .Where(ia => ia.IsActive && !ia.IsAcknowledged)
                .OrderByDescending(ia => ia.AlertDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<InventoryAlert>> GetAlertsByTypeAsync(string alertType, CancellationToken cancellationToken)
        {
            return await _context.InventoryAlerts
                .Include(ia => ia.Item)
                .Include(ia => ia.Godown)
                .Where(ia => ia.AlertType == alertType)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<InventoryAlert>> GetAlertsByItemAsync(int itemId, CancellationToken cancellationToken)
        {
            return await _context.InventoryAlerts
                .Include(ia => ia.Godown)
                .Where(ia => ia.ItemId == itemId)
                .ToListAsync(cancellationToken);
        }

        public async Task<PagedResult<InventoryAlert>> GetPagedAlertsAsync(int pageNumber, int pageSize, string? searchTerm, CancellationToken cancellationToken)
        {
            var query = _context.Set<InventoryAlert>()
                .Include(ia => ia.Item)
                .Include(ia => ia.Godown)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(ia => ia.Item!.ItemName.Contains(searchTerm) || 
                                         ia.AlertType.Contains(searchTerm) ||
                                         ia.Message.Contains(searchTerm));
            }

            var totalCount = await query.CountAsync(cancellationToken);
            var items = await query
                .OrderByDescending(ia => ia.AlertDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PagedResult<InventoryAlert>(items, totalCount, pageNumber, pageSize);
        }

        Task<Application.DTOs.Common.PagedResult<InventoryAlert>> IInventoryAlertRepository.GetPagedAlertsAsync(int pageNumber, int pageSize, string? searchTerm, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task AddAsync(InventoryAlert inventoryAlert)
        {
            throw new NotImplementedException();
        }

        public Task GetPagedInventoryAlertsAsync(int pageNumber, int pageSize, string? alertType, string? severity, bool? isActive, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}