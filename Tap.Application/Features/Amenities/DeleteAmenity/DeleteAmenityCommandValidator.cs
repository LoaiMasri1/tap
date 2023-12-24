using FluentValidation;
using Tap.Application.Core.Errors;
using Tap.Application.Core.Extensions;

namespace Tap.Application.Features.Amenities.DeleteAmenity;

public class DeleteAmenityCommandValidator : AbstractValidator<DeleteAmenityCommand>
{
    public DeleteAmenityCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithError(ValidationErrors.DeleteAmenity.AmenityIdRequired);
    }
}
