using FluentValidation;
using Application.DTOs.Supplier;

namespace Application.DTOs.Supplier.Validators
{
    public class UpdateSupplierValidator : AbstractValidator<UpdateSupplierDto>
    {
        public UpdateSupplierValidator()
        {
            RuleFor(p => p.Id)
                .GreaterThan(0).WithMessage("ID must be a valid ID.");

            RuleFor(p => p.SupplierName)
                .MaximumLength(100).When(p => !string.IsNullOrEmpty(p.SupplierName)).WithMessage("Supplier name cannot exceed 100 characters.");

            RuleFor(p => p.ContactPerson)
                .MaximumLength(100).When(p => !string.IsNullOrEmpty(p.ContactPerson)).WithMessage("Contact person cannot exceed 100 characters.");

            RuleFor(p => p.Email)
                .EmailAddress().When(p => !string.IsNullOrEmpty(p.Email)).WithMessage("A valid email address is required.");

            RuleFor(p => p.PhoneNumber)
                .MaximumLength(20).When(p => !string.IsNullOrEmpty(p.PhoneNumber)).WithMessage("Phone number cannot exceed 20 characters.");

            RuleFor(p => p.Address)
                .MaximumLength(200).When(p => !string.IsNullOrEmpty(p.Address)).WithMessage("Address cannot exceed 200 characters.");
        }
    }
}