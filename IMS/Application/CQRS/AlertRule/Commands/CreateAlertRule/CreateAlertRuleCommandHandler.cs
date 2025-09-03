using MediatR;
using Application.Contracts;
using Application.DTOs.AlertRule.Validators;
using Application.Responses;
using AutoMapper;

namespace Application.CQRS.AlertRule.Commands.CreateAlertRule
{
    public class CreateAlertRuleCommandHandler : IRequestHandler<CreateAlertRuleCommand, BaseCommandResponse>
    {
        private readonly IAlertRuleRepository _alertRuleRepository;
        private readonly IMapper _mapper;

        public CreateAlertRuleCommandHandler(IAlertRuleRepository alertRuleRepository, IMapper mapper)
        {
            _alertRuleRepository = alertRuleRepository;
            _mapper = mapper;
        }

        public async Task<BaseCommandResponse> Handle(CreateAlertRuleCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();
            var validator = new CreateAlertRuleValidator();
            var validationResult = await validator.ValidateAsync(request.AlertRuleDto, cancellationToken);

            if (!validationResult.IsValid)
            {
                response.Success = false;
                response.Message = "Alert rule creation failed due to validation errors.";
                response.Errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return response;
            }

            var alertRule = _mapper.Map<Domain.Models.AlertRule>(request.AlertRuleDto);
            var addedAlertRule = await _alertRuleRepository.AddAsync(alertRule, cancellationToken);

            response.Success = true;
            response.Message = "Alert rule created successfully.";
            response.Id = addedAlertRule.Id;

            return response;
        }
    }
}