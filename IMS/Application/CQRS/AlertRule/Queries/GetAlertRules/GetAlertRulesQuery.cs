using Application.DTOs.AlertRule;
using MediatR;

namespace Application.CQRS.AlertRule.Queries.GetAlertRules
{
    public class GetAlertRulesQuery : IRequest<IReadOnlyList<AlertRuleDto>>
    {
        // Add filtering, paging, sorting parameters if needed
    }
}