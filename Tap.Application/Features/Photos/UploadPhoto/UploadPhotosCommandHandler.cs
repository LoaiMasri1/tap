using MediatR;
using Tap.Application.Core.Abstractions.Common;
using Tap.Application.Core.Abstractions.Data;
using Tap.Contracts.Features.Photos;
using Tap.Domain.Core.Primitives.Result;
using Tap.Domain.Features.Photos;

namespace Tap.Application.Features.Photos.UploadPhoto;

public class UploadPhotosCommandHandler
    : IRequestHandler<UploadPhotosCommand, Result<UploadPhotosResponse>>
{
    private readonly IUploadFileService _uploadFileService;
    private readonly IPhotoRepository _photoRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UploadPhotosCommandHandler(
        IUploadFileService uploadFileService,
        IPhotoRepository photoRepository,
        IUnitOfWork unitOfWork
    )
    {
        _uploadFileService = uploadFileService;
        _photoRepository = photoRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<UploadPhotosResponse>> Handle(
        UploadPhotosCommand request,
        CancellationToken cancellationToken
    )
    {
        var filesResult = request.Files.ValidateFileRequest();

        if (filesResult.IsFailure)
        {
            return filesResult.Error;
        }

        var fileResponses = await _uploadFileService.UploadFilesAsync(request.Files);

        var photosResult = fileResponses
            .Select(
                fileResponse => Photo.Create(fileResponse.Url, request.ItemType, request.ItemId)
            )
            .ToArray();

        var result = Result.Combine(photosResult);

        if (result.IsFailure)
        {
            return result.Error;
        }

        var photos = photosResult.Select(x => x.Value).ToArray();

        _photoRepository.InsertRange(photos);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var urls = fileResponses.Select(x => x.Url).ToArray();

        return new UploadPhotosResponse(urls);
    }
}
