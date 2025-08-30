using FluentValidation;
using Application.DTOs.Invoice;

namespace Application.DTOs.Invoice.Validators
{
    public class CreateInvoiceDetailValidator : AbstractValidator<CreateInvoiceDetailDto>
    {
        public CreateInvoiceDetailValidator()
        {
            RuleFor(p => p.ItemId)
                .GreaterThan(0).WithMessage("Item ID must be a valid ID.");

            RuleFor(p => p.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be a positive value.");

            RuleFor(p => p.UnitPrice)
                .GreaterThanOrEqualTo(0).WithMessage("Unit price must be a non-negative value.");
        }
    }
}