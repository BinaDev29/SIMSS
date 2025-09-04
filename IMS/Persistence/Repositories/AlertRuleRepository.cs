// Persistence/Repositories/AlertRuleRepository.cs
using Application.Contracts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
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

        public async Task<IReadOnlyList<AlertRule>> GetActiveRulesAsync(CancellationToken cancellationToken)
        {
            return await _context.AlertRules
                .Where(ar => ar.IsActive)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<AlertRule>> GetRulesByTypeAsync(string ruleType, CancellationToken cancellationToken)
        {
            return await _context.AlertRules
                .Where(ar => ar.RuleType == ruleType)
                .ToListAsync(cancellationToken);
        }

        public async Task<PagedResult<AlertRule>> GetPagedRulesAsync(int pageNumber, int pageSize, string? searchTerm, CancellationToken cancellationToken)
        {
            var query = _context.Set<AlertRule>().AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(ar => ar.RuleName.Contains(searchTerm) || 
                                         ar.RuleType.Contains(searchTerm) ||
                                         ar.AlertMessage.Contains(searchTerm));
            }

            var totalCount = await query.CountAsync(cancellationToken);
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PagedResult<AlertRule>(items, totalCount, pageNumber, pageSize);
        }

        Task<Application.DTOs.Common.PagedResult<AlertRule>> IAlertRuleRepository.GetPagedRulesAsync(int pageNumber, int pageSize, string? searchTerm, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public object Query()
        {
            throw new NotImplementedException();
        }
    }
}