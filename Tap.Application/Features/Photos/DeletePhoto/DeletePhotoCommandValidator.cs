using FluentValidation;
using Tap.Application.Core.Errors;
using Tap.Application.Core.Extensions;

namespace Tap.Application.Features.Photos.DeletePhoto;

public class DeletePhotoCommandValidator : AbstractValidator<DeletePhotoCommand>
{
    public DeletePhotoCommandValidator()
    {
        RuleFor(x => x.PhotoId)
            .NotEmpty()
            .GreaterThan(0)
            .WithError(ValidationErrors.DeletePhoto.PhotoIdRequired);
    }
}
