using FluentValidation;
using Application.DTOs.Invoice;

namespace Application.DTOs.Invoice.Validators
{
    public class InvoiceDetailValidator : AbstractValidator<InvoiceDetailDto>
    {
        public InvoiceDetailValidator()
        {
            RuleFor(p => p.Id)
                .GreaterThan(0).WithMessage("ID must be a valid ID.");

            RuleFor(p => p.InvoiceId)
                .GreaterThan(0).WithMessage("Invoice ID must be a valid ID.");

            RuleFor(p => p.ItemId)
                .GreaterThan(0).WithMessage("Item ID must be a valid ID.");

            RuleFor(p => p.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be a positive value.");

            RuleFor(p => p.UnitPrice)
                .GreaterThanOrEqualTo(0).WithMessage("Unit price must be a non-negative value.");
        }
    }
}