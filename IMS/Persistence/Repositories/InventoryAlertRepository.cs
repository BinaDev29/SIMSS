// Persistence/Repositories/InventoryAlertRepository.cs
using Application.Contracts;
using Application.DTOs.Common;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class InventoryAlertRepository : GenericRepository<InventoryAlert>, IInventoryAlertRepository
    {
        public InventoryAlertRepository(SIMSDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<PagedResult<InventoryAlert>> GetPagedInventoryAlertsAsync(int pageNumber, int pageSize, string? alertType, string? severity, bool? isActive, CancellationToken cancellationToken)
        {
            var query = _context.Set<InventoryAlert>().AsQueryable();

            if (!string.IsNullOrEmpty(alertType))
            {
                query = query.Where(a => a.AlertType == alertType);
            }

            if (!string.IsNullOrEmpty(severity))
            {
                query = query.Where(a => a.Severity == severity);
            }

            if (isActive.HasValue)
            {
                query = query.Where(a => a.IsActive == isActive.Value);
            }

            var totalCount = await query.CountAsync(cancellationToken);
            var items = await query
                .Include(a => a.Item)
                .Include(a => a.Godown)
                .OrderByDescending(a => a.AlertDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PagedResult<InventoryAlert>(items, totalCount, pageNumber, pageSize);
        }

        public async Task<IReadOnlyList<InventoryAlert>> GetActiveAlertsByItemAsync(int itemId, CancellationToken cancellationToken)
        {
            return await _context.Set<InventoryAlert>()
                .Where(a => a.ItemId == itemId && a.IsActive)
                .Include(a => a.Item)
                .Include(a => a.Godown)
                .OrderByDescending(a => a.AlertDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<InventoryAlert>> GetActiveAlertsByGodownAsync(int godownId, CancellationToken cancellationToken)
        {
            return await _context.Set<InventoryAlert>()
                .Where(a => a.GodownId == godownId && a.IsActive)
                .Include(a => a.Item)
                .Include(a => a.Godown)
                .OrderByDescending(a => a.AlertDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<InventoryAlert>> GetCriticalAlertsAsync(CancellationToken cancellationToken)
        {
            return await _context.Set<InventoryAlert>()
                .Where(a => a.Severity == "CRITICAL" && a.IsActive && !a.IsAcknowledged)
                .Include(a => a.Item)
                .Include(a => a.Godown)
                .OrderByDescending(a => a.AlertDate)
                .ToListAsync(cancellationToken);
        }

        public async Task AcknowledgeAlertAsync(int alertId, string acknowledgedBy, CancellationToken cancellationToken)
        {
            var alert = await _context.Set<InventoryAlert>().FindAsync(new object[] { alertId }, cancellationToken);
            if (alert != null)
            {
                alert.IsAcknowledged = true;
                alert.AcknowledgedDate = DateTime.UtcNow;
                alert.AcknowledgedBy = acknowledgedBy;
            }
        }
    }
}