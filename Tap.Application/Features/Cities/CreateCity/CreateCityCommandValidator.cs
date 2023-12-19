using FluentValidation;
using Tap.Application.Core.Errors;
using Tap.Application.Core.Extentions;

namespace Tap.Application.Features.Cities.CreateCity;

public class CreateCityCommandValidator : AbstractValidator<CreateCityCommand>
{
    public CreateCityCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithError(ValidationErrors.CreateCity.NameIsRequired)
            .MaximumLength(50)
            .WithError(ValidationErrors.CreateCity.NameTooLong);

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithError(ValidationErrors.CreateCity.DescriptionIsRequired)
            .MaximumLength(500)
            .WithError(ValidationErrors.CreateCity.DescriptionTooLong);

        RuleFor(x => x.Country)
            .NotEmpty()
            .WithError(ValidationErrors.CreateCity.CountryIsRequired)
            .MaximumLength(50)
            .WithError(ValidationErrors.CreateCity.CountryTooLong);
    }
}
