using Application.DTOs.Item;
using FluentValidation;

namespace Application.DTOs.Item.Validators
{
    public class CreateItemValidator : AbstractValidator<CreateItemDto>
    {
        public CreateItemValidator()
        {
            RuleFor(p => p.ItemName)
                .NotEmpty().WithMessage("Item name is required.")
                .MaximumLength(100).WithMessage("Item name cannot exceed 100 characters.");

            RuleFor(p => p.ItemCode)
                .NotEmpty().WithMessage("Item code is required.")
                .MaximumLength(50).WithMessage("Item code cannot exceed 50 characters.");

            RuleFor(p => p.StockQuantity)
                .GreaterThanOrEqualTo(0).WithMessage("Stock quantity must be a non-negative value.");

            RuleFor(p => p.UnitOfMeasure)
                .NotEmpty().WithMessage("Unit of measure is required.")
                .MaximumLength(20).WithMessage("Unit of measure cannot exceed 20 characters.");

            RuleFor(p => p.PurchasePrice)
                .GreaterThanOrEqualTo(0).WithMessage("Purchase price must be a non-negative value.");

            RuleFor(p => p.SalePrice)
                .GreaterThanOrEqualTo(0).WithMessage("Sale price must be a non-negative value.");

            RuleFor(p => p.ExpiryDate)
                .GreaterThanOrEqualTo(p => p.ManufacturingDate)
                .When(p => p.ManufacturingDate.HasValue && p.ExpiryDate.HasValue)
                .WithMessage("Expiry date must be on or after the manufacturing date.");
        }
    }
}