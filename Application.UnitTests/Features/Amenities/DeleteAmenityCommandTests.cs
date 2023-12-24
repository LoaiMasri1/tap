using FluentAssertions;
using NSubstitute;
using Tap.Application.Core.Abstractions.Data;
using Tap.Application.Features.Amenities.DeleteAmenity;
using Tap.Application.Features.Authentication;
using Tap.Domain.Core.Errors;
using Tap.Domain.Core.Primitives.Maybe;
using Tap.Domain.Features.Amenities;
using Tap.Domain.Features.Users;

namespace Application.UnitTests.Features.Amenities;

public class DeleteAmenityCommandTests
{
    private readonly IAmenityRepository _amenityRepositoryMock;
    private readonly IUserIdentifierProvider _userIdentifierProviderMock;
    private readonly IUnitOfWork _unitOfWorkMock;

    private static readonly DeleteAmenityCommand Command = new(1);

    private readonly DeleteAmenityCommandHandler _sut;

    public DeleteAmenityCommandTests()
    {
        _amenityRepositoryMock = Substitute.For<IAmenityRepository>();
        _userIdentifierProviderMock = Substitute.For<IUserIdentifierProvider>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();

        _sut = new DeleteAmenityCommandHandler(
            _unitOfWorkMock,
            _amenityRepositoryMock,
            _userIdentifierProviderMock
        );

        _amenityRepositoryMock
            .GetByIdAsync(Command.Id, CancellationToken.None)
            .Returns(
                Maybe<Amenity>.From(Amenity.Create("name", "description", AmenityType.Hotel, 1))
            );
    }

    [Fact]
    public async Task Handle_WhenUserIsNotAdmin_ShouldReturnUnauthorizedError()
    {
        // Arrange
        _userIdentifierProviderMock.Role.Returns(UserRole.User);

        // Act
        var result = await _sut.Handle(Command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.User.Unauthorized);
    }

    [Fact]
    public async Task Handle_WhenUserIsAdmin_ShouldReturnSuccessResult()
    {
        // Arrange
        _userIdentifierProviderMock.Role.Returns(UserRole.Admin);
        _userIdentifierProviderMock.Id.Returns(1);

        // Act
        var result = await _sut.Handle(Command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_WhenUserIsAdmin_ShouldCallRemove()
    {
        // Arrange
        _userIdentifierProviderMock.Role.Returns(UserRole.Admin);
        _userIdentifierProviderMock.Id.Returns(1);

        // Act
        await _sut.Handle(Command, CancellationToken.None);

        // Assert
        _amenityRepositoryMock.Received(1).Remove(Arg.Any<Amenity>());
    }

    [Fact]
    public async Task Handle_WhenUserIsAdmin_ShouldCallSaveChangesAsync()
    {
        // Arrange
        _userIdentifierProviderMock.Role.Returns(UserRole.Admin);
        _userIdentifierProviderMock.Id.Returns(1);

        // Act
        await _sut.Handle(Command, CancellationToken.None);

        // Assert
        await _unitOfWorkMock.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenUserIsAdminAndAmenityDoesNotExist_ShouldReturnError()
    {
        // Arrange
        _userIdentifierProviderMock.Role.Returns(UserRole.Admin);
        _userIdentifierProviderMock.Id.Returns(1);

        _amenityRepositoryMock.GetByIdAsync(1, CancellationToken.None).Returns(Maybe<Amenity>.None);

        // Act
        var result = await _sut.Handle(Command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.Amenity.NotFound);
    }

    [Fact]
    public async Task Handle_WhenUserIsAdminAndAmenityExists_ShouldReturnSuccessResult()
    {
        // Arrange
        _userIdentifierProviderMock.Role.Returns(UserRole.Admin);
        _userIdentifierProviderMock.Id.Returns(1);

        // Act
        var result = await _sut.Handle(Command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_WhenUserIsAdminAndAmenityExists_ShouldCallRemove()
    {
        // Arrange
        _userIdentifierProviderMock.Role.Returns(UserRole.Admin);
        _userIdentifierProviderMock.Id.Returns(1);

        // Act
        await _sut.Handle(Command, CancellationToken.None);

        // Assert
        _amenityRepositoryMock.Received(1).Remove(Arg.Any<Amenity>());
    }
}
