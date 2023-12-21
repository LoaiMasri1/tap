using FluentAssertions;
using NSubstitute;
using Tap.Application.Core.Abstractions.Common;
using Tap.Application.Core.Abstractions.Data;
using Tap.Application.Features.Photos.UploadPhoto;
using Tap.Contracts.Files;
using Tap.Domain.Common.Enumerations;
using Tap.Domain.Core.Errors;
using Tap.Domain.Features.Photos;

namespace Application.UnitTests.Features.Photos;

public class UploadPhotosTests
{
    private readonly IPhotoRepository _photoRepositoryMock;
    private readonly IUploadFileService _uploadFileServiceMock;
    private readonly IUnitOfWork _unitOfWorkMock;

    private static readonly UploadPhotosCommand Command =
        new(1, ItemType.Hotel, new FileRequest[] { new("test", "test", new[] { byte.MaxValue, }) });

    private readonly UploadPhotosCommandHandler _sut;

    public UploadPhotosTests()
    {
        _photoRepositoryMock = Substitute.For<IPhotoRepository>();
        _uploadFileServiceMock = Substitute.For<IUploadFileService>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();

        _sut = new UploadPhotosCommandHandler(
            _uploadFileServiceMock,
            _photoRepositoryMock,
            _unitOfWorkMock
        );
    }

    [Fact]
    public async Task Handle_WhenFilesMoreThan5_ReturnsTooManyFilesError()
    {
        // Arrange
        var files = new FileRequest[]
        {
            new("test", "", new[] { byte.MaxValue, }),
            new("test", "test", new[] { byte.MaxValue, }),
            new("test", "test", new[] { byte.MaxValue, }),
            new("test", "test", new[] { byte.MaxValue, }),
            new("test", "test", new[] { byte.MaxValue, }),
            new("test", "test", new[] { byte.MaxValue, }),
        };

        var command = Command with { Files = files };

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.Error.Should().Be(DomainErrors.Photo.EnsurePhotosCount);
    }

    [Fact]
    public async Task Handle_WhenFilesAreNotImages_ReturnsInvalidFileTypeError()
    {
        // Arrange
        var files = new FileRequest[] { new("test", "", new[] { byte.MaxValue, }), };
        var command = Command with { Files = files };

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.Error.Should().Be(DomainErrors.Photo.EnsurePhotosType);
    }

    [Fact]
    public async Task Handle_WhenFileIsMoreThan5MB_ReturnsFileIsTooLargeError()
    {
        // Arrange
        var files = new FileRequest[] { new("test", "image/png", new byte[5 * 1024 * 1024 + 1]), };

        var command = Command with { Files = files };

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.Error.Should().Be(DomainErrors.Photo.EnsurePhotosSize);
    }

    [Fact]
    public async Task Handle_WhenFilesAreValid_ReturnsUrls()
    {
        // Arrange
        var files = new FileRequest[] { new("test", "image/png", new byte[5 * 1024 * 1024]), };

        var command = Command with { Files = files };

        var fileResponses = new FileResponse[] { new("test", "test") };

        _uploadFileServiceMock.UploadFilesAsync(Arg.Any<FileRequest[]>()).Returns(fileResponses);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.Value.Urls.Should().BeEquivalentTo(fileResponses.Select(x => x.Url));
    }

    [Theory]
    [InlineData(ItemType.Hotel)]
    [InlineData(ItemType.City)]
    [InlineData(ItemType.Room)]
    public async Task Handle_WhenFilesAreValid_ReturnsSuccess(ItemType itemType)
    {
        // Arrange
        var files = new FileRequest[] { new("test", "image/png", new byte[5 * 1024 * 1024]), };

        var command = Command with { Files = files, ItemType = itemType };

        var fileResponses = new FileResponse[] { new("test", "test") };

        _uploadFileServiceMock.UploadFilesAsync(Arg.Any<FileRequest[]>()).Returns(fileResponses);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_WhenFilesAreValid_ShouldCallSaveChangesAsync()
    {
        // Arrange
        var files = new FileRequest[] { new("test", "image/png", new byte[5 * 1024 * 1024]), };

        var command = Command with { Files = files };

        var fileResponses = new FileResponse[] { new("test", "test") };

        _uploadFileServiceMock.UploadFilesAsync(Arg.Any<FileRequest[]>()).Returns(fileResponses);

        // Act
        await _sut.Handle(command, CancellationToken.None);

        // Assert
        await _unitOfWorkMock.Received().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenFilesAreValid_ShouldCallInsertRange()
    {
        // Arrange
        var files = new FileRequest[] { new("test", "image/png", new byte[5 * 1024 * 1024]), };

        var command = Command with { Files = files };

        var fileResponses = new FileResponse[] { new("test", "test") };

        _uploadFileServiceMock.UploadFilesAsync(Arg.Any<FileRequest[]>()).Returns(fileResponses);

        // Act
        await _sut.Handle(command, CancellationToken.None);

        // Assert
        _photoRepositoryMock.Received().InsertRange(Arg.Any<Photo[]>());
    }
}
