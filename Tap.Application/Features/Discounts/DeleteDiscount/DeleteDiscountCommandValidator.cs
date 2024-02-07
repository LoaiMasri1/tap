using FluentValidation;
using Tap.Application.Core.Errors;
using Tap.Application.Core.Extensions;

namespace Tap.Application.Features.Discounts.DeleteDiscount;

public class DeleteDiscountCommandValidator : AbstractValidator<DeleteDiscountCommand>
{
    public DeleteDiscountCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThanOrEqualTo(0)
            .WithError(ValidationErrors.DeleteDiscount.DiscountIdIsRequired);
    }
}
