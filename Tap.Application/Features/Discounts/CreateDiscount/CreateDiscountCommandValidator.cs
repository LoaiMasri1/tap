using FluentValidation;
using Tap.Application.Core.Errors;
using Tap.Application.Core.Extensions;

namespace Tap.Application.Features.Discounts.CreateDiscount;

public class CreateDiscountCommandValidator : AbstractValidator<CreateDiscountCommand>
{
    public CreateDiscountCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithError(ValidationErrors.CreateDiscount.NameIsRequired);
        RuleFor(x => x.Description)
            .NotEmpty()
            .WithError(ValidationErrors.CreateDiscount.DescriptionIsRequired);

        RuleFor(x => x.DiscountPercentage)
            .NotEmpty()
            .WithError(ValidationErrors.CreateDiscount.DiscountPercentageIsRequired);
    }
}
