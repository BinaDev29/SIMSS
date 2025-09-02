// Persistence/Repositories/AlertRuleRepository.cs
using Application.Contracts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class AlertRuleRepository : GenericRepository<AlertRule>, IAlertRuleRepository
    {
        private readonly SIMSDbContext _context;

        public AlertRuleRepository(SIMSDbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

        public async Task<IReadOnlyList<AlertRule>> GetAllActiveRulesAsync(CancellationToken cancellationToken)
        {
            return await _context.AlertRules
                .Where(r => r.IsActive)
                .ToListAsync(cancellationToken);
        }

        public async Task<AlertRule?> GetRuleByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.AlertRules.FindAsync(new object[] { id }, cancellationToken);
        }
    }
}
