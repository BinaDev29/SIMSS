using FluentValidation;

namespace Application.DTOs.Godown.Validators
{
    public class UpdateGodownValidator : AbstractValidator<UpdateGodownDto>
    {
        public UpdateGodownValidator()
        {
            RuleFor(p => p.Id)
                .GreaterThan(0).WithMessage("ID must be a valid ID.");

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