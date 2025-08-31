// Application/Contracts/IAlertRuleRepository.cs
using Application.DTOs.Common;
using Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface IAlertRuleRepository : IGenericRepository<AlertRule>
    {
        Task<PagedResult<AlertRule>> GetPagedAlertRulesAsync(int pageNumber, int pageSize, string? ruleType, bool? isActive, CancellationToken cancellationToken);
        Task<IReadOnlyList<AlertRule>> GetActiveAlertRulesAsync(CancellationToken cancellationToken);
        Task<IReadOnlyList<AlertRule>> GetAlertRulesByTypeAsync(string ruleType, CancellationToken cancellationToken);
        Task UpdateLastTriggeredAsync(int alertRuleId, CancellationToken cancellationToken);
    }
}