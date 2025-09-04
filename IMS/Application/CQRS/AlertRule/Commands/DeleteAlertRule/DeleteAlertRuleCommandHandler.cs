using MediatR;
using Application.Contracts;
using Application.Responses;

namespace Application.CQRS.AlertRule.Commands.DeleteAlertRule
{
    public class DeleteAlertRuleCommandHandler : IRequestHandler<DeleteAlertRuleCommand, BaseCommandResponse>
    {
        private readonly IAlertRuleRepository _alertRuleRepository;

        public DeleteAlertRuleCommandHandler(IAlertRuleRepository alertRuleRepository)
        {
            _alertRuleRepository = alertRuleRepository;
        }

        public async Task<BaseCommandResponse> Handle(DeleteAlertRuleCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();
            var alertRule = await _alertRuleRepository.GetByIdAsync(request.Id, cancellationToken);

            if (alertRule == null)
            {
                response.Success = false;
                response.Message = "Alert rule not found for deletion.";
                return response;
            }

            await _alertRuleRepository.DeleteAsync(alertRule, cancellationToken);
            response.Success = true;
            response.Message = "Alert rule deleted successfully.";
            return response;
        }
    }
}