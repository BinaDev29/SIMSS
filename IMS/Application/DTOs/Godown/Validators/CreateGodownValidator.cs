using FluentValidation;

namespace Application.DTOs.Godown.Validators
{
    public class CreateGodownValidator : AbstractValidator<CreateGodownDto>
    {
        public CreateGodownValidator()
        {
            RuleFor(p => p.GodownName)
                .NotEmpty().WithMessage("Godown name is required.")
                .MaximumLength(100).WithMessage("Godown name cannot exceed 100 characters.");

            RuleFor(p => p.Location)
                .NotEmpty().WithMessage("Location is required.")
                .MaximumLength(100).WithMessage("Location cannot exceed 100 characters.");

            RuleFor(p => p.GodownManager)
                .MaximumLength(100).WithMessage("Godown manager name cannot exceed 100 characters.");
        }
    }
}