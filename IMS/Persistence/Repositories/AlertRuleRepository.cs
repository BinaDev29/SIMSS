// Persistence/Repositories/AlertRuleRepository.cs
using Application.Contracts;
using Application.DTOs.Common;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class AlertRuleRepository : GenericRepository<AlertRule>, IAlertRuleRepository
    {
        public AlertRuleRepository(SIMSDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<PagedResult<AlertRule>> GetPagedAlertRulesAsync(int pageNumber, int pageSize, string? ruleType, bool? isActive, CancellationToken cancellationToken)
        {
            var query = _context.Set<AlertRule>().AsQueryable();

            if (!string.IsNullOrEmpty(ruleType))
            {
                query = query.Where(r => r.RuleType == ruleType);
            }

            if (isActive.HasValue)
            {
                query = query.Where(r => r.IsActive == isActive.Value);
            }

            var totalCount = await query.CountAsync(cancellationToken);
            var items = await query
                .OrderBy(r => r.RuleName)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PagedResult<AlertRule>(items, totalCount, pageNumber, pageSize);
        }

        public async Task<IReadOnlyList<AlertRule>> GetActiveAlertRulesAsync(CancellationToken cancellationToken)
        {
            return await _context.Set<AlertRule>()
                .Where(r => r.IsActive)
                .OrderBy(r => r.RuleName)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<AlertRule>> GetAlertRulesByTypeAsync(string ruleType, CancellationToken cancellationToken)
        {
            return await _context.Set<AlertRule>()
                .Where(r => r.RuleType == ruleType && r.IsActive)
                .OrderBy(r => r.RuleName)
                .ToListAsync(cancellationToken);
        }

        public async Task UpdateLastTriggeredAsync(int alertRuleId, CancellationToken cancellationToken)
        {
            var alertRule = await _context.Set<AlertRule>().FindAsync(new object[] { alertRuleId }, cancellationToken);
            if (alertRule != null)
            {
                alertRule.LastTriggered = DateTime.UtcNow;
                alertRule.TriggerCount++;
            }
        }
    }
}