using FluentValidation;

namespace Application.DTOs.Delivery.Validators
{
    public class CreateDeliveryDetailValidator : AbstractValidator<CreateDeliveryDetailDto>
    {
        public CreateDeliveryDetailValidator()
        {
            RuleFor(x => x.ItemId)
                .GreaterThan(0).WithMessage("Item ID must be a valid ID.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than 0.");
        }
    }
}