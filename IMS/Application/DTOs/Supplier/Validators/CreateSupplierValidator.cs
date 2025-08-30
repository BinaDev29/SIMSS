using FluentValidation;
using Application.DTOs.Supplier;

namespace Application.DTOs.Supplier.Validators
{
    public class CreateSupplierValidator : AbstractValidator<CreateSupplierDto>
    {
        public CreateSupplierValidator()
        {
            RuleFor(p => p.SupplierName)
                .NotEmpty().WithMessage("Supplier name is required.")
                .MaximumLength(100).WithMessage("Supplier name cannot exceed 100 characters.");

            RuleFor(p => p.ContactPerson)
                .NotEmpty().WithMessage("Contact person is required.")
                .MaximumLength(100).WithMessage("Contact person cannot exceed 100 characters.");

            RuleFor(p => p.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("A valid email address is required.");

            RuleFor(p => p.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required.")
                .MaximumLength(20).WithMessage("Phone number cannot exceed 20 characters.");

            RuleFor(p => p.Address)
                .MaximumLength(200).WithMessage("Address cannot exceed 200 characters.");
        }
    }
}