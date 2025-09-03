using Application.DTOs.AlertRule;
using Application.Responses;
using MediatR;

namespace Application.CQRS.AlertRule.Commands.UpdateAlertRule
{
    public class UpdateAlertRuleCommand : IRequest<BaseCommandResponse>
    {
        public required UpdateAlertRuleDto AlertRuleDto { get; set; }
    }
}