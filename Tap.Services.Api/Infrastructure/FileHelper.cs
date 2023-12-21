using Tap.Contracts.Files;
using Tap.Services.Api.Extensions;

namespace Tap.Services.Api.Infrastructure;

public static class FileHelper
{
    public static FileRequest[] CreateFileRequest(this IFormCollection files) =>
        files
            .Files
            .Select(
                x => new FileRequest(x.FileName, x.ContentType, x.OpenReadStream().ReadAllBytes())
            )
            .ToArray();

    public static FileRequest CreateFileRequest(this IFormFile file) =>
        new(file.FileName, file.ContentType, file.OpenReadStream().ReadAllBytes());
}
