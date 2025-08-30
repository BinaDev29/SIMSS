using Application.DTOs.Employee;
using FluentValidation;

namespace Application.DTOs.Employee.Validators
{
    public class UpdateEmployeeValidator : AbstractValidator<UpdateEmployeeDto>
    {
        public UpdateEmployeeValidator()
        {
            RuleFor(p => p.Id)
                .GreaterThan(0).WithMessage("ID must be a valid ID.");

            RuleFor(p => p.FirstName)
                .MaximumLength(50).WithMessage("First name cannot exceed 50 characters.");

            RuleFor(p => p.LastName)
                .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters.");

            RuleFor(p => p.Email)
                .EmailAddress().When(p => !string.IsNullOrEmpty(p.Email)).WithMessage("Invalid email address format.");

            RuleFor(p => p.PhoneNumber)
                .MaximumLength(20).When(p => !string.IsNullOrEmpty(p.PhoneNumber)).WithMessage("Phone number cannot exceed 20 characters.");

            RuleFor(p => p.JobTitle)
                .MaximumLength(50).When(p => !string.IsNullOrEmpty(p.JobTitle)).WithMessage("Job title cannot exceed 50 characters.");

            RuleFor(p => p.Department)
                .MaximumLength(50).When(p => !string.IsNullOrEmpty(p.Department)).WithMessage("Department cannot exceed 50 characters.");

            RuleFor(p => p.Salary)
                .GreaterThanOrEqualTo(0).When(p => p.Salary.HasValue).WithMessage("Salary must be a positive value.");
        }
    }
}