using MediatR;
using Application.Contracts;
using Application.DTOs.AlertRule.Validators;
using Application.Responses;
using AutoMapper;

namespace Application.CQRS.AlertRule.Commands.UpdateAlertRule
{
    public class UpdateAlertRuleCommandHandler : IRequestHandler<UpdateAlertRuleCommand, BaseCommandResponse>
    {
        private readonly IAlertRuleRepository _alertRuleRepository;
        private readonly IMapper _mapper;

        public UpdateAlertRuleCommandHandler(IAlertRuleRepository alertRuleRepository, IMapper mapper)
        {
            _alertRuleRepository = alertRuleRepository;
            _mapper = mapper;
        }

        public async Task<BaseCommandResponse> Handle(UpdateAlertRuleCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();
            var validator = new UpdateAlertRuleValidator();
            var validationResult = await validator.ValidateAsync(request.AlertRuleDto, cancellationToken);

            if (!validationResult.IsValid)
            {
                response.Success = false;
                response.Message = "Alert rule update failed due to validation errors.";
                response.Errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return response;
            }

            var alertRule = await _alertRuleRepository.GetByIdAsync(request.AlertRuleDto.Id, cancellationToken);
            if (alertRule == null)
            {
                response.Success = false;
                response.Message = "Alert rule not found.";
                return response;
            }

            _mapper.Map(request.AlertRuleDto, alertRule);
            await _alertRuleRepository.Update(alertRule, cancellationToken);

            response.Success = true;
            response.Message = "Alert rule updated successfully.";
            return response;
        }
    }
}