using FluentValidation;
using Application.DTOs.Customer;
namespace Application.DTOs.Customer.Validators
{
    public class CreateCustomerValidator : AbstractValidator<CreateCustomerDto>
    {
        public CreateCustomerValidator()
        {
            RuleFor(x => x.CustomerName)
                .NotEmpty().WithMessage("Customer name is required.")
                .MaximumLength(100);

            RuleFor(x => x.ContactPerson)
                .NotEmpty().WithMessage("Contact person is required.")
                .MaximumLength(100);

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.")
                .MaximumLength(100);

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required.")
                .MaximumLength(20);

            RuleFor(x => x.Address)
                .MaximumLength(250);

            RuleFor(x => x.TaxId)
                .MaximumLength(50);

            RuleFor(x => x.PaymentTerms)
                .MaximumLength(100);
        }
    }
}