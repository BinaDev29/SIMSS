using FluentValidation;

namespace Application.DTOs.GodownInventory.Validators
{
    public class CreateGodownInventoryValidator : AbstractValidator<CreateGodownInventoryDto>
    {
        public CreateGodownInventoryValidator()
        {
            RuleFor(p => p.GodownId)
                .GreaterThan(0).WithMessage("Godown ID must be a valid ID.");

            RuleFor(p => p.ItemId)
                .GreaterThan(0).WithMessage("Item ID must be a valid ID.");

            RuleFor(p => p.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than 0.");
        }
    }
}