using FluentValidation;
using Tap.Application.Core.Errors;
using Tap.Application.Core.Extensions;

namespace Tap.Application.Features.Photos.UploadPhoto;

public class UploadPhotosCommandValidator : AbstractValidator<UploadPhotosCommand>
{
    public UploadPhotosCommandValidator()
    {
        RuleFor(x => x.ItemId)
            .NotEmpty()
            .WithError(ValidationErrors.UploadPhotos.ItemIdRequired)
            .GreaterThan(0)
            .WithError(ValidationErrors.UploadPhotos.ItemIdRequired);
        RuleFor(x => x.Files).NotEmpty().WithError(ValidationErrors.UploadPhotos.FilesRequired);
    }
}
