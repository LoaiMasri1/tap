using FluentValidation;
using Tap.Application.Core.Errors;
using Tap.Application.Core.Extensions;

namespace Tap.Application.Features.Discounts.CreateDiscount;

public class CreateDiscountCommandValidator : AbstractValidator<CreateDiscountCommand>
{
    private IRuleBuilderOptions<CreateDiscountCommand, string> _ruleBuilderOptions;

    public CreateDiscountCommandValidator()
    {
        _ruleBuilderOptions = RuleFor(x => x.Name)
            .NotEmpty()
            .WithError(ValidationErrors.CreateDiscount.NameIsRequired);
        RuleFor(x => x.Description)
            .NotEmpty()
            .WithError(ValidationErrors.CreateDiscount.DescriptionIsRequired);

        RuleFor(x => x.DiscountPercentage)
            .NotEmpty()
            .WithError(ValidationErrors.CreateDiscount.DiscountPercentageIsRequired);

        RuleFor(x => x.StartDate)
            .NotEmpty()
            .WithError(ValidationErrors.CreateDiscount.StartDateIsRequired)
            .GreaterThanOrEqualTo(DateTime.UtcNow)
            .WithError(ValidationErrors.CreateDiscount.StartDateMustBeGreaterThanOrEqualToToday);

        RuleFor(x => x.EndDate)
            .NotEmpty()
            .WithError(ValidationErrors.CreateDiscount.EndDateIsRequired)
            .GreaterThanOrEqualTo(DateTime.UtcNow)
            .WithError(ValidationErrors.CreateDiscount.EndDateMustBeGreaterThanOrEqualToToday);

        RuleFor(x => x.EndDate)
            .GreaterThanOrEqualTo(x => x.StartDate)
            .WithError(ValidationErrors.CreateDiscount.EndDateMustBeGreaterThanOrEqualToStartDate);
    }
}
