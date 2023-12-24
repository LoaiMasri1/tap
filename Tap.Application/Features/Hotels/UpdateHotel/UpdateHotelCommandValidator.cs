using FluentValidation;
using Tap.Application.Core.Errors;
using Tap.Application.Core.Extensions;

namespace Tap.Application.Features.Hotels.UpdateHotel;

public class UpdateHotelCommandValidator : AbstractValidator<UpdateHotelCommand>
{
    public UpdateHotelCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithError(ValidationErrors.UpdateHotel.HotelIdRequired);
        RuleFor(x => x.Name).NotEmpty().WithError(ValidationErrors.UpdateHotel.NameIsRequired);

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithError(ValidationErrors.UpdateHotel.DescriptionIsRequired);
        RuleFor(x => x.Latitude)
            .NotEmpty()
            .WithError(ValidationErrors.UpdateHotel.LatitudeIsRequired);
        RuleFor(x => x.Longitude)
            .NotEmpty()
            .WithError(ValidationErrors.UpdateHotel.LongitudeIsRequired);
    }
}
