using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.User.Validators
{
    public class UpdateUserValidator : AbstractValidator<UpdateUserDto>
    {
        public UpdateUserValidator()
        {
            RuleFor(p => p.Id)
                .GreaterThan(0).WithMessage("ID must be a valid ID.");

            RuleFor(p => p.Username)
                .MaximumLength(50).When(p => p.Username != null).WithMessage("Username cannot exceed 50 characters.");

            RuleFor(p => p.Role)
                .MaximumLength(50).When(p => p.Role != null).WithMessage("Role cannot exceed 50 characters.");
        }
    }
}