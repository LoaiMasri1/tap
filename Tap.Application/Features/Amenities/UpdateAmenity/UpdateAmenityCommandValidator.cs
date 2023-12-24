using FluentValidation;
using Tap.Application.Core.Errors;
using Tap.Application.Core.Extensions;

namespace Tap.Application.Features.Amenities.UpdateAmenity;

public class UpdateAmenityCommandValidator : AbstractValidator<UpdateAmenityCommand>
{
    public UpdateAmenityCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithError(ValidationErrors.UpdateAmenity.AmenityIdRequired);
        RuleFor(x => x.Name).NotEmpty().WithError(ValidationErrors.UpdateAmenity.NameIsRequired);
        RuleFor(x => x.Description)
            .NotEmpty()
            .WithError(ValidationErrors.UpdateAmenity.DescriptionIsRequired);
    }
}
