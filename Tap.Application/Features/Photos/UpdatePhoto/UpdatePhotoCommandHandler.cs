using Tap.Application.Core.Abstractions.Common;
using Tap.Application.Core.Abstractions.Data;
using Tap.Application.Core.Messaging;
using Tap.Contracts.Features.Photos;
using Tap.Domain.Core.Errors;
using Tap.Domain.Core.Primitives.Result;
using Tap.Domain.Features.Photos;

namespace Tap.Application.Features.Photos.UpdatePhoto;

public class UpdatePhotoCommandHandler
    : ICommandHandler<UpdatePhotoCommand, Result<UpdatePhotoResponse>>
{
    private readonly IPhotoRepository _photoRepository;
    private readonly IUploadFileService _uploadFileService;
    private readonly IUnitOfWork _unitOfWork;

    public UpdatePhotoCommandHandler(
        IPhotoRepository photoRepository,
        IUploadFileService uploadFileService,
        IUnitOfWork unitOfWork
    )
    {
        _photoRepository = photoRepository;
        _uploadFileService = uploadFileService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<UpdatePhotoResponse>> Handle(
        UpdatePhotoCommand request,
        CancellationToken cancellationToken
    )
    {
        var maybePhoto = await _photoRepository.GetByIdAsync(request.PhotoId, cancellationToken);

        if (maybePhoto.HasNoValue)
        {
            return DomainErrors.Photo.PhotoNotFound;
        }

        var photo = maybePhoto.Value;

        var fileResult = request.File.ValidateFileRequest();

        if (fileResult.IsFailure)
        {
            return fileResult.Error;
        }

        var fileResponse = await _uploadFileService.UploadFilesAsync(new[] { request.File });

        photo.UpdateUrl(fileResponse[0].Url);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new UpdatePhotoResponse(photo.Url);
    }
}
