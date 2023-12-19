using Tap.Application.Core.Abstractions.Common;
using Tap.Contracts.Files;

namespace Tap.Infrastructure.Common;

public class LocalUploadFileService : IUploadFileService
{
    private readonly IFileService _fileService;

    public LocalUploadFileService(IFileService fileService) => _fileService = fileService;

    public async Task<FileResponse[]> UploadFilesAsync(
        FileRequest[] files,
        string? folderName = null
    )
    {
        var fileNames = await _fileService.SaveFilesAsync(files, folderName);
        return _fileService.GetFilesUrl(fileNames, folderName);
    }
}
