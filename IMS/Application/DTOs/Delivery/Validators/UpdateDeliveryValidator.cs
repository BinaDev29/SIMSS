using FluentValidation;
using Application.DTOs.Delivery;

namespace Application.DTOs.Delivery.Validators
{
    public class UpdateDeliveryValidator : AbstractValidator<UpdateDeliveryDto>
    {
        public UpdateDeliveryValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("ID must be a valid ID.");

            RuleFor(x => x.DeliveryNumber)
                .NotEmpty().WithMessage("Delivery number is required.")
                .MaximumLength(50).WithMessage("Delivery number cannot exceed 50 characters.");

            RuleFor(x => x.DeliveryDate)
                .NotEmpty().WithMessage("Delivery date is required.");

            RuleFor(x => x.CustomerId)
                .GreaterThan(0).WithMessage("Customer ID must be a valid ID.");

            RuleFor(x => x.DeliveredByEmployeeId)
                .GreaterThan(0).WithMessage("Employee ID must be a valid ID.");

            RuleFor(x => x.Status)
                .MaximumLength(50).WithMessage("Status cannot exceed 50 characters.");

            // የdelivery ዝርዝሮችን ለመፈተሽ RuleForEachን እንጠቀማለን።
            RuleForEach(x => x.DeliveryDetails).SetValidator(new UpdateDeliveryDetailValidator());
        }
    }
}