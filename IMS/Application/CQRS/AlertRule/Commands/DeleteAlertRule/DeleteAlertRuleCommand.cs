using Application.Responses;
using MediatR;

namespace Application.CQRS.AlertRule.Commands.DeleteAlertRule
{
    public class DeleteAlertRuleCommand : IRequest<BaseCommandResponse>
    {
        public required int Id { get; set; }
    }
}