// Application/DTOs/Customer/Validators/CreateCustomerValidator.cs
using FluentValidation;

namespace Application.DTOs.Customer.Validators
{
    public class CreateCustomerValidator : AbstractValidator<CreateCustomerDto>
    {
        public CreateCustomerValidator()
        {
            RuleFor(x => x.CustomerName)
                .NotEmpty().WithMessage("Customer name is required.")
                .MaximumLength(100).WithMessage("Customer name cannot exceed 100 characters.");

            RuleFor(x => x.ContactPerson)
                .NotEmpty().WithMessage("Contact person is required.")
                .MaximumLength(100).WithMessage("Contact person cannot exceed 100 characters.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required.")
                .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Invalid phone number format.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email address format.");

            RuleFor(x => x.Address)
                .MaximumLength(200).WithMessage("Address cannot exceed 200 characters.")
                .When(x => !string.IsNullOrEmpty(x.Address));

            RuleFor(x => x.TaxId)
                .MaximumLength(50).WithMessage("Tax ID cannot exceed 50 characters.")
                .When(x => !string.IsNullOrEmpty(x.TaxId));

            RuleFor(x => x.PaymentTerms)
                .MaximumLength(100).WithMessage("Payment terms cannot exceed 100 characters.")
                .When(x => !string.IsNullOrEmpty(x.PaymentTerms));
        }
    }
}