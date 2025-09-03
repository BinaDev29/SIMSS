// Application/DTOs/AlertRule/Validators/UpdateAlertRuleValidator.cs
using FluentValidation;
using Application.DTOs.AlertRule;

namespace Application.DTOs.AlertRule.Validators
{
    public class UpdateAlertRuleValidator : AbstractValidator<UpdateAlertRuleDto>
    {
        public UpdateAlertRuleValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required.")
                .GreaterThan(0);

            RuleFor(x => x.RuleName)
                .NotEmpty().WithMessage("Rule name is required.")
                .MaximumLength(100);

            RuleFor(x => x.RuleType)
                .NotEmpty().WithMessage("Rule type is required.")
                .MaximumLength(50);

            RuleFor(x => x.Condition)
                .NotEmpty().WithMessage("Condition is required.");

            RuleFor(x => x.AlertMessage)
                .NotEmpty().WithMessage("Alert message is required.");

            RuleFor(x => x.Priority)
                .MaximumLength(20);
        }
    }
}