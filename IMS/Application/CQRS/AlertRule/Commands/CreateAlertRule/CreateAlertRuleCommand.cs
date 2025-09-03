using Application.DTOs.AlertRule;
using Application.Responses;
using MediatR;

namespace Application.CQRS.AlertRule.Commands.CreateAlertRule
{
    public class CreateAlertRuleCommand : IRequest<BaseCommandResponse>
    {
        public required CreateAlertRuleDto AlertRuleDto { get; set; }
    }
}