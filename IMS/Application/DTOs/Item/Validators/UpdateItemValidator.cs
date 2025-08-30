using Application.DTOs.Item;
using FluentValidation;

namespace Application.DTOs.Item.Validators
{
    public class UpdateItemValidator : AbstractValidator<UpdateItemDto>
    {
        public UpdateItemValidator()
        {
            RuleFor(p => p.Id)
                .GreaterThan(0).WithMessage("ID must be a valid ID.");

            RuleFor(p => p.ItemName)
                .MaximumLength(100).When(p => !string.IsNullOrEmpty(p.ItemName)).WithMessage("Item name cannot exceed 100 characters.");

            RuleFor(p => p.ItemCode)
                .MaximumLength(50).When(p => !string.IsNullOrEmpty(p.ItemCode)).WithMessage("Item code cannot exceed 50 characters.");

            RuleFor(p => p.StockQuantity)
                .GreaterThanOrEqualTo(0).When(p => p.StockQuantity.HasValue).WithMessage("Stock quantity must be a non-negative value.");

            RuleFor(p => p.UnitOfMeasure)
                .MaximumLength(20).When(p => !string.IsNullOrEmpty(p.UnitOfMeasure)).WithMessage("Unit of measure cannot exceed 20 characters.");

            RuleFor(p => p.PurchasePrice)
                .GreaterThanOrEqualTo(0).When(p => p.PurchasePrice.HasValue).WithMessage("Purchase price must be a non-negative value.");

            RuleFor(p => p.SalePrice)
                .GreaterThanOrEqualTo(0).When(p => p.SalePrice.HasValue).WithMessage("Sale price must be a non-negative value.");

            RuleFor(p => p.ExpiryDate)
                .GreaterThanOrEqualTo(p => p.ManufacturingDate)
                .When(p => p.ManufacturingDate.HasValue && p.ExpiryDate.HasValue)
                .WithMessage("Expiry date must be on or after the manufacturing date.");
        }
    }
}