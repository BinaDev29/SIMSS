using FluentValidation;
using Application.DTOs.Delivery;

namespace Application.DTOs.Delivery.Validators
{
    public class UpdateDeliveryDetailValidator : AbstractValidator<UpdateDeliveryDetailDto>
    {
        public UpdateDeliveryDetailValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("ID must be a valid ID.");

            RuleFor(x => x.DeliveryId)
                .GreaterThan(0).WithMessage("Delivery ID must be a valid ID.");

            RuleFor(x => x.ItemId)
                .GreaterThan(0).WithMessage("Item ID must be a valid ID.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than 0.");
        }
    }
}