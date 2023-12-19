using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Tap.Application.Core.Abstractions.Common;
using Tap.Contracts.Files;

namespace Tap.Infrastructure.Common;

public class FileService : IFileService
{
    private readonly IWebHostEnvironment _env;
    private readonly IDateTime _dateTime;
    private readonly ITokenGenerator _guidGenerator;
    private ILogger<FileService> _logger;

    private static readonly object Lock = new();

    public FileService(
        IWebHostEnvironment env,
        IDateTime dateTime,
        ITokenGenerator guidGenerator,
        ILogger<FileService> logger
    )
    {
        _env = env;
        _dateTime = dateTime;
        _guidGenerator = guidGenerator;
        _logger = logger;
    }

    private const string DefaultFolderName = "wwwroot";
    private const string ImagesFolderName = "images";

    public async Task<string[]> SaveFilesAsync(FileRequest[] files, string? folderName = null)
    {
        folderName ??= DefaultFolderName;
        var path = Path.Combine(_env.ContentRootPath, folderName, ImagesFolderName);

        _logger.LogInformation("Saving {Count} files to {Path}", files.Length, path);

        lock (Lock)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        var fileNames = new List<string>();
        var tasks = new List<Task>();

        foreach (var file in files)
        {
            var fileName = GenerateUniqueFileName(Path.GetExtension(file.FileName));
            var filePath = Path.Combine(path, fileName);
            tasks.Add(File.WriteAllBytesAsync(filePath, file.Content));
            fileNames.Add(fileName);
        }

        await Task.WhenAll(tasks);

        tasks.Clear();

        return fileNames.ToArray();
    }

    public FileResponse[] GetFilesUrl(string[] fileNames, string? folderName = null) =>
        fileNames
            .Select(fileName => new FileResponse(fileName, GetFileUrl(fileName, folderName)))
            .ToArray();

    public string GenerateUniqueFileName(string fileName) =>
        $"{_dateTime.UtcNow:yyyy_mm_dd__hh_mm}_{_guidGenerator.Generate()}_{fileName}";

    public static string GetFileUrl(string fileName, string? folderName = null) =>
        Path.Combine(folderName ?? DefaultFolderName, fileName);
}
