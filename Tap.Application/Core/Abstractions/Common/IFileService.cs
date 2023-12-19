using Tap.Contracts.Files;

namespace Tap.Application.Core.Abstractions.Common;

public interface IFileService
{
    public Task<string[]> SaveFilesAsync(FileRequest[] files, string? folderName = null);
    public FileResponse[] GetFilesUrl(string[] fileNames, string? folderName = null);
    public string GenerateUniqueFileName(string fileName);
}
