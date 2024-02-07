using FluentValidation;
using Tap.Application.Core.Errors;
using Tap.Application.Core.Extensions;

namespace Tap.Application.Features.Hotels.CreateHotel;

public class CreateHotelCommandValidator : AbstractValidator<CreateHotelCommand>
{
    public CreateHotelCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithError(ValidationErrors.CreateHotel.NameIsRequired);

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithError(ValidationErrors.CreateHotel.DescriptionIsRequired);

        RuleFor(x => x.Latitude)
            .NotEmpty()
            .WithError(ValidationErrors.CreateHotel.LatitudeIsRequired);

        RuleFor(x => x.Longitude)
            .NotEmpty()
            .WithError(ValidationErrors.CreateHotel.LongitudeIsRequired);

        RuleFor(x => x.CityId).NotEmpty().WithError(ValidationErrors.CreateHotel.CityIdIsRequired);
    }
}
