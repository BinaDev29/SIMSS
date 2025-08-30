using FluentValidation;

namespace Application.DTOs.User.Validators
{
    public class CreateUserValidator : AbstractValidator<CreateUserDto>
    {
        public CreateUserValidator()
        {
            RuleFor(p => p.Username)
                .NotEmpty().WithMessage("Username is required.")
                .MaximumLength(50).WithMessage("Username cannot exceed 50 characters.");

            RuleFor(p => p.PasswordHash)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.");

            RuleFor(p => p.Role)
                .NotEmpty().WithMessage("Role is required.")
                .MaximumLength(50).WithMessage("Role cannot exceed 50 characters.");
        }
    }
}