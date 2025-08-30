using FluentValidation;
using Application.DTOs.Transaction;

namespace Application.DTOs.Transaction.Validators
{
    public class UpdateReturnTransactionValidator : AbstractValidator<UpdateReturnTransactionDto>
    {
        public UpdateReturnTransactionValidator()
        {
            RuleFor(p => p.Id)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0.");

            RuleFor(p => p.GodownId)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0.");

            RuleFor(p => p.ItemId)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0.");

            RuleFor(p => p.CustomerId)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0.");

            RuleFor(p => p.Quantity)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0.");

            RuleFor(p => p.ReturnDate)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .LessThanOrEqualTo(DateTime.Now).WithMessage("{PropertyName} cannot be in the future.");

            RuleFor(p => p.Reason)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MaximumLength(250).WithMessage("{PropertyName} cannot exceed 250 characters.");

            RuleFor(p => p.EmployeeId)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0.");
        }
    }
}