using FluentValidation;
using Application.DTOs.Invoice;

namespace Application.DTOs.Invoice.Validators
{
    public class UpdateInvoiceDetailValidator : AbstractValidator<UpdateInvoiceDetailDto>
    {
        public UpdateInvoiceDetailValidator()
        {
            RuleFor(p => p.Id)
                .GreaterThan(0).WithMessage("ID must be a valid ID.");

            RuleFor(p => p.InvoiceId)
                .GreaterThan(0).When(p => p.InvoiceId.HasValue).WithMessage("Invoice ID must be a valid ID.");

            RuleFor(p => p.ItemId)
                .GreaterThan(0).When(p => p.ItemId.HasValue).WithMessage("Item ID must be a valid ID.");

            RuleFor(p => p.Quantity)
                .GreaterThan(0).When(p => p.Quantity.HasValue).WithMessage("Quantity must be a positive value.");

            RuleFor(p => p.UnitPrice)
                .GreaterThanOrEqualTo(0).When(p => p.UnitPrice.HasValue).WithMessage("Unit price must be a non-negative value.");
        }
    }
}