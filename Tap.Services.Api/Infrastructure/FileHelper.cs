using Tap.Contracts.Files;
using Tap.Services.Api.Extensions;

namespace Tap.Services.Api.Infrastructure;

public static class FileHelper
{
    private const int MaxFileSize = 5 * 1024 * 1024;

    public static FileRequest[] CreateFileRequest(this IFormCollection files) =>
        files
            .Files
            .Select(x => new FileRequest(x.FileName, x.OpenReadStream().ReadAllBytes()))
            .ToArray();

    public static bool IsImageFile(this IFormCollection files) => files.Files.Any(IsImage);

    private static Func<IFormFile, bool> IsImage =>
        file => file.ContentType is "image/jpeg" or "image/png";

    public static bool IsExceededSize(this IFormCollection files, int size = MaxFileSize) =>
        files.Files.Any(file => file.Length > size);
}
