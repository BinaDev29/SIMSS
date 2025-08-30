using FluentValidation;

namespace Application.DTOs.Common.Validators
{
    public class PagedRequestValidator : AbstractValidator<PagedRequestDto>
    {
        public PagedRequestValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThan(0).WithMessage("Page number must be greater than 0.");

            RuleFor(x => x.PageSize)
                .GreaterThan(0).WithMessage("Page size must be greater than 0.")
                .LessThanOrEqualTo(100).WithMessage("Page size must be 100 or less.");
        }
    }
}