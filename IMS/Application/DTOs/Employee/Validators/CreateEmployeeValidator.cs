using Application.DTOs.Employee;
using FluentValidation;

namespace Application.DTOs.Employee.Validators
{
    public class CreateEmployeeValidator : AbstractValidator<CreateEmployeeDto>
    {
        public CreateEmployeeValidator()
        {
            RuleFor(p => p.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .MaximumLength(50).WithMessage("First name cannot exceed 50 characters.");

            RuleFor(p => p.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters.");

            RuleFor(p => p.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email address format.");

            RuleFor(p => p.PhoneNumber)
                .MaximumLength(20).WithMessage("Phone number cannot exceed 20 characters.");

            RuleFor(p => p.JobTitle)
                .MaximumLength(50).WithMessage("Job title cannot exceed 50 characters.");

            RuleFor(p => p.Department)
                .MaximumLength(50).WithMessage("Department cannot exceed 50 characters.");

            RuleFor(p => p.Salary)
                .GreaterThanOrEqualTo(0).When(p => p.Salary.HasValue).WithMessage("Salary must be a positive value.");

            RuleFor(p => p.UserId)
                .NotEmpty().WithMessage("User ID is required.");
        }
    }
}