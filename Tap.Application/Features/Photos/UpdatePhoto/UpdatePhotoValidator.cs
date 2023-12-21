using FluentValidation;
using Tap.Application.Core.Errors;
using Tap.Application.Core.Extentions;

namespace Tap.Application.Features.Photos.UpdatePhoto;

public class UpdatePhotoValidator : AbstractValidator<UpdatePhotoCommand>
{
    public UpdatePhotoValidator()
    {
        RuleFor(x => x.PhotoId).NotEmpty().WithError(ValidationErrors.UpdatePhoto.PhotoIdRequired);
        RuleFor(x => x.File).NotEmpty().WithError(ValidationErrors.UpdatePhoto.FileRequired);
    }
}
