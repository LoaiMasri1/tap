using Azure.Storage.Blobs;
using Microsoft.Extensions.Options;
using Tap.Application.Core.Abstractions.Common;
using Tap.Contracts.Files;
using Tap.Infrastructure.Common.Options;

namespace Tap.Infrastructure.Common;

public class AzureBlobService : IUploadFileService
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly AzureBlobOptions _options;
    private readonly IFileService _fileService;

    public AzureBlobService(
        BlobServiceClient blobServiceClient,
        IOptions<AzureBlobOptions> options,
        IFileService fileService
    )
    {
        _blobServiceClient = blobServiceClient;
        _fileService = fileService;
        _options = options.Value;
    }

    public async Task<FileResponse[]> UploadFilesAsync(
        FileRequest[] files,
        string? folderName = null
    )
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(_options.ContainerName);
        var fileResponses = new List<FileResponse>();

        foreach (var file in files)
        {
            var fileName = _fileService.GenerateUniqueFileName(Path.GetExtension(file.FileName));

            var blobClient = containerClient.GetBlobClient(fileName);

            await blobClient.UploadAsync(new MemoryStream(file.Content));

            fileResponses.Add(new FileResponse(file.FileName, blobClient.Uri.AbsoluteUri));
        }

        return fileResponses.ToArray();
    }
}
