using MediatR;
using Tap.Application.Core.Abstractions.Common;
using Tap.Application.Core.Abstractions.Data;
using Tap.Domain.Core.Errors;
using Tap.Domain.Core.Primitives.Result;
using Tap.Domain.Features.Photos;

namespace Tap.Application.Features.Photos.DeletePhoto;

public class DeletePhotoCommandHandler : IRequestHandler<DeletePhotoCommand, Result>
{
    private readonly IPhotoRepository _photoRepository;
    private readonly IFileService _fileService;
    private readonly IUnitOfWork _unitOfWork;

    public DeletePhotoCommandHandler(
        IPhotoRepository photoRepository,
        IFileService fileService,
        IUnitOfWork unitOfWork
    )
    {
        _photoRepository = photoRepository;
        _fileService = fileService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(
        DeletePhotoCommand request,
        CancellationToken cancellationToken
    )
    {
        var maybePhoto = await _photoRepository.GetByIdAsync(request.PhotoId, cancellationToken);

        if (maybePhoto.HasNoValue)
        {
            return DomainErrors.Photo.PhotoNotFound;
        }

        var photo = maybePhoto.Value;

        _photoRepository.Remove(photo);

        //_fileService.DeleteFiles(new[] { photo.Url });
        // TODO: uncomment this line when we have a way to delete files from the file system and azure blob storage

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
