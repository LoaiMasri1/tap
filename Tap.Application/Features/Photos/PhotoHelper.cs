using Tap.Contracts.Files;
using Tap.Domain.Core.Errors;
using Tap.Domain.Core.Primitives.Result;

namespace Tap.Application.Features.Photos;

public static class PhotoHelper
{
    private const int MaxPhotoSize = 5 * 1024 * 1024; // 5 MB
    private static readonly string[] PhotoTypes = { "image/jpeg", "image/png" };
    private const int MaxPhotoCount = 5;

    public static readonly Func<FileRequest[], bool> EnsurePhotosCount = x =>
        x.Length is > 0 and <= 5;

    public static readonly Func<FileRequest[], bool> EnsurePhotosType = x =>
        x.All(file => file.ContentType is "image/jpeg" or "image/png");

    public static readonly Func<FileRequest[], bool> EnsurePhotosSize = x =>
        x.All(file => file.Content.Length <= MaxPhotoSize);

    public static Result<FileRequest[]> CreateFileRequest(this FileRequest[] files) =>
        Result
            .Create(files)
            .Ensure(x => x.Length is > 0 and <= MaxPhotoCount, DomainErrors.Photo.EnsurePhotosCount)
            .Ensure(
                x => x.All(file => PhotoTypes.Contains(file.ContentType)),
                DomainErrors.Photo.EnsurePhotosType
            )
            .Ensure(
                x => x.All(file => file.Content.Length <= MaxPhotoSize),
                DomainErrors.Photo.EnsurePhotosSize
            );

    public static Result<FileRequest> CreateFileRequest(this FileRequest file) =>
        Result
            .Create(file)
            .Ensure(x => PhotoTypes.Contains(x.ContentType), DomainErrors.Photo.EnsurePhotosType)
            .Ensure(x => x.Content.Length <= MaxPhotoSize, DomainErrors.Photo.EnsurePhotosSize);
}
