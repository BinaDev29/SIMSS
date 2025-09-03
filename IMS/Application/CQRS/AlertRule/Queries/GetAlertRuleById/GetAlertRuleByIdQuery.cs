using Application.DTOs.AlertRule;
using MediatR;

namespace Application.CQRS.AlertRule.Queries.GetAlertRuleById
{
    public class GetAlertRuleByIdQuery : IRequest<AlertRuleDto?>
    {
        public required int Id { get; set; }
    }
}