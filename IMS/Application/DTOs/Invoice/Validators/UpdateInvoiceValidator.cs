using FluentValidation;
using Application.DTOs.Invoice;

namespace Application.DTOs.Invoice.Validators
{
    public class UpdateInvoiceValidator : AbstractValidator<UpdateInvoiceDto>
    {
        public UpdateInvoiceValidator()
        {
            RuleFor(p => p.Id)
                .GreaterThan(0).WithMessage("ID must be a valid ID.");

            RuleFor(p => p.InvoiceNumber)
                .MaximumLength(50).When(p => !string.IsNullOrEmpty(p.InvoiceNumber)).WithMessage("Invoice number cannot exceed 50 characters.");

            RuleFor(p => p.CustomerId)
                .GreaterThan(0).When(p => p.CustomerId.HasValue).WithMessage("Customer ID must be a valid ID.");

            RuleFor(p => p.EmployeeId)
                .GreaterThan(0).When(p => p.EmployeeId.HasValue).WithMessage("Employee ID must be a valid ID.");

            RuleFor(p => p.TotalAmount)
                .GreaterThanOrEqualTo(0).When(p => p.TotalAmount.HasValue).WithMessage("Total amount must be a non-negative value.");

            // Nested collections are validated only if they are not null
            RuleForEach(p => p.InvoiceDetails).SetValidator(new UpdateInvoiceDetailValidator()).When(p => p.InvoiceDetails != null);
        }
    }
}