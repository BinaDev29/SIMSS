// Application/Contracts/IAlertRuleRepository.cs
using Application.DTOs.Common;
using Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface IAlertRuleRepository : IGenericRepository<AlertRule>
    {
        Task<IReadOnlyList<AlertRule>> GetActiveRulesAsync(CancellationToken cancellationToken);
        Task<IReadOnlyList<AlertRule>> GetRulesByTypeAsync(string ruleType, CancellationToken cancellationToken);
        Task<PagedResult<AlertRule>> GetPagedRulesAsync(int pageNumber, int pageSize, string? searchTerm, CancellationToken cancellationToken);
    }
}