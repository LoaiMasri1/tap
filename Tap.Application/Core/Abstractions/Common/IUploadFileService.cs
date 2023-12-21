using Tap.Contracts.Files;

namespace Tap.Application.Core.Abstractions.Common;

public interface IUploadFileService
{
    Task<FileResponse[]> UploadFilesAsync(FileRequest[] files, string? folderName = null);
}
