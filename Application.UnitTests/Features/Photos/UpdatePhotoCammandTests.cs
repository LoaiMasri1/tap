using FluentAssertions;
using NSubstitute;
using Tap.Application.Core.Abstractions.Common;
using Tap.Application.Core.Abstractions.Data;
using Tap.Application.Features.Photos.UpdatePhoto;
using Tap.Contracts.Features.Photos;
using Tap.Contracts.Files;
using Tap.Domain.Common.Enumerations;
using Tap.Domain.Core.Errors;
using Tap.Domain.Core.Primitives.Maybe;
using Tap.Domain.Features.Photos;

namespace Application.UnitTests.Features.Photos;

public class UpdatePhotoCommandTests
{
    private readonly IPhotoRepository _photoRepositoryMock;
    private readonly IUploadFileService _uploadFileServiceMock;
    private readonly IUnitOfWork _unitOfWorkMock;

    private static readonly UpdatePhotoCommand Command =
        new(1, new FileRequest("test", "image/png", new[] { byte.MinValue, }));

    private static readonly Photo Photo = Photo.Create("test", ItemType.City, 1).Value;

    private readonly UpdatePhotoCommandHandler _sut;

    public UpdatePhotoCommandTests()
    {
        _photoRepositoryMock = Substitute.For<IPhotoRepository>();
        _uploadFileServiceMock = Substitute.For<IUploadFileService>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();

        _sut = new UpdatePhotoCommandHandler(
            _photoRepositoryMock,
            _uploadFileServiceMock,
            _unitOfWorkMock
        );
    }

    [Fact]
    public async Task Handle_GivenValidRequest_ShouldReturnSuccessResult()
    {
        // Arrange
        _photoRepositoryMock
            .GetByIdAsync(Command.PhotoId, Arg.Any<CancellationToken>())
            .Returns(Maybe<Photo>.From(Photo));

        _uploadFileServiceMock
            .UploadFilesAsync(Arg.Any<FileRequest[]>())
            .Returns(new[] { new FileResponse("test", "test") });

        // Act
        var result = await _sut.Handle(Command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    // return photo not found error
    [Fact]
    public async Task Handle_GivenInvalidPhotoId_ShouldReturnPhotoNotFoundError()
    {
        // Arrange
        _photoRepositoryMock
            .GetByIdAsync(Command.PhotoId, Arg.Any<CancellationToken>())
            .Returns(Maybe<Photo>.None);

        // Act
        var result = await _sut.Handle(Command, CancellationToken.None);

        // Assert
        result.Error.Should().Be(DomainErrors.Photo.PhotoNotFound);
    }

    // return invalid file error
    [Fact]
    public async Task Handle_GivenInvalidFile_ShouldReturnInvalidFileError()
    {
        // Arrange
        _photoRepositoryMock
            .GetByIdAsync(Command.PhotoId, Arg.Any<CancellationToken>())
            .Returns(Maybe<Photo>.From(Photo));

        var command = Command with
        {
            File = new FileRequest("test", "application/pdf", new[] { byte.MinValue, })
        };

        _uploadFileServiceMock
            .UploadFilesAsync(Arg.Any<FileRequest[]>())
            .Returns(new[] { new FileResponse("test", "test") });

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.Error.Should().Be(DomainErrors.Photo.EnsurePhotosType);
    }

    [Fact]
    public async Task Handle_GivenValidRequest_ShouldReturnUpdatePhotoResponse()
    {
        // Arrange
        _photoRepositoryMock
            .GetByIdAsync(Command.PhotoId, Arg.Any<CancellationToken>())
            .Returns(Maybe<Photo>.From(Photo));

        _uploadFileServiceMock
            .UploadFilesAsync(Arg.Any<FileRequest[]>())
            .Returns(new[] { new FileResponse("test", "test") });

        // Act
        var result = await _sut.Handle(Command, CancellationToken.None);

        // Assert
        result.Value.Should().BeOfType<UpdatePhotoResponse>();
    }

    [Fact]
    public async Task Handle_GivenValidRequest_ShouldUpdatePhotoUrl()
    {
        // Arrange
        _photoRepositoryMock
            .GetByIdAsync(Command.PhotoId, Arg.Any<CancellationToken>())
            .Returns(Maybe<Photo>.From(Photo));

        _uploadFileServiceMock
            .UploadFilesAsync(Arg.Any<FileRequest[]>())
            .Returns(new[] { new FileResponse("test", "test") });

        // Act
        var result = await _sut.Handle(Command, CancellationToken.None);

        // Assert
        result.Value.Url.Should().Be(Photo.Url);
    }

    [Fact]
    public async Task Handle_GivenValidRequest_ShouldSaveChanges()
    {
        // Arrange
        _photoRepositoryMock
            .GetByIdAsync(Command.PhotoId, Arg.Any<CancellationToken>())
            .Returns(Maybe<Photo>.From(Photo));

        _uploadFileServiceMock
            .UploadFilesAsync(Arg.Any<FileRequest[]>())
            .Returns(new[] { new FileResponse("test", "test") });

        // Act
        await _sut.Handle(Command, CancellationToken.None);

        // Assert
        await _unitOfWorkMock.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
