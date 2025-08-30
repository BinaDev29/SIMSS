using FluentValidation;
using Application.DTOs.Invoice;

namespace Application.DTOs.Invoice.Validators
{
    public class CreateInvoiceValidator : AbstractValidator<CreateInvoiceDto>
    {
        public CreateInvoiceValidator()
        {
            RuleFor(p => p.InvoiceNumber)
                .NotEmpty().WithMessage("Invoice number is required.")
                .MaximumLength(50).WithMessage("Invoice number cannot exceed 50 characters.");

            RuleFor(p => p.CustomerId)
                .GreaterThan(0).WithMessage("Customer ID must be a valid ID.");

            RuleFor(p => p.EmployeeId)
                .GreaterThan(0).WithMessage("Employee ID must be a valid ID.");

            RuleFor(p => p.InvoiceDate)
                .NotEmpty().WithMessage("Invoice date is required.");

            RuleFor(p => p.DueDate)
                .NotEmpty().WithMessage("Due date is required.");

            RuleFor(p => p.TotalAmount)
                .GreaterThanOrEqualTo(0).WithMessage("Total amount must be a non-negative value.");

            RuleForEach(p => p.InvoiceDetails).SetValidator(new CreateInvoiceDetailValidator());
        }
    }
}