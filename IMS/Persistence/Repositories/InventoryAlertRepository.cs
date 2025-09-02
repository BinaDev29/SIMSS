// Persistence/Repositories/InventoryAlertRepository.cs
using Application.Contracts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
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
                .Where(a => a.IsActive)
                .Include(a => a.Item)
                .Include(a => a.Godown)
                .ToListAsync(cancellationToken);
        }

        public async Task AcknowledgeAlertAsync(int alertId, string acknowledgedBy, CancellationToken cancellationToken)
        {
            var alert = await _context.InventoryAlerts.FindAsync(new object[] { alertId }, cancellationToken);
            if (alert != null)
            {
                alert.IsAcknowledged = true;
                alert.AcknowledgedBy = acknowledgedBy;
                alert.AcknowledgedDate = DateTime.UtcNow;
            }
        }
    }
}
