using FluentAssertions;
using NSubstitute;
using Tap.Application.Core.Abstractions.Data;
using Tap.Application.Features.Amenities.CreateAmenity;
using Tap.Application.Features.Authentication;
using Tap.Domain.Core.Errors;
using Tap.Domain.Core.Primitives.Result;
using Tap.Domain.Features.Amenities;
using Tap.Domain.Features.Users;

namespace Application.UnitTests.Features.Amenities;

public class CreateAmenityCommandTests
{
    private readonly IAmenityRepository _amenityRepositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;
    private readonly IAmenityService _amenityServiceMock;
    private readonly IUserContext _userContextMock;

    private static readonly CreateAmenityCommand Command =
        new("Amenity Name", "Amenity Description", AmenityType.Hotel, 1);

    private readonly CreateAmenityCommandHandler _sut;

    public CreateAmenityCommandTests()
    {
        _amenityRepositoryMock = Substitute.For<IAmenityRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();
        _amenityServiceMock = Substitute.For<IAmenityService>();
        _userContextMock = Substitute.For<IUserContext>();

        _sut = new CreateAmenityCommandHandler(
            _amenityRepositoryMock,
            _unitOfWorkMock,
            _amenityServiceMock,
            _userContextMock
        );
    }

    [Fact]
    public async Task Handle_WhenUserIsNotAdmin_ShouldReturnUnauthorizedError()
    {
        // Arrange
        _userContextMock.Role.Returns(UserRole.User);

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
        _userContextMock.Role.Returns(UserRole.Admin);
        _userContextMock.Id.Returns(1);

        _amenityServiceMock
            .CheckAmenityTypeAndUserOwnerShipAsync(1, 1, AmenityType.Hotel, CancellationToken.None)
            .Returns(Result.Success());

        // Act
        var result = await _sut.Handle(Command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_WhenUserIsAdminAndAmenityTypeAndUserOwnershipCheckFails_ShouldReturnError()
    {
        // Arrange
        _userContextMock.Role.Returns(UserRole.Admin);
        _userContextMock.Id.Returns(1);

        _amenityServiceMock
            .CheckAmenityTypeAndUserOwnerShipAsync(1, 1, AmenityType.Hotel, CancellationToken.None)
            .Returns(DomainErrors.Amenity.NothingToUpdate);

        // Act
        var result = await _sut.Handle(Command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.Amenity.NothingToUpdate);
    }

    [Fact]
    public async Task Handle_WhenUserIsAdminAndAmenityTypeAndUserOwnershipCheckSucceeds_ShouldReturnSuccessResult()
    {
        // Arrange
        _userContextMock.Role.Returns(UserRole.Admin);
        _userContextMock.Id.Returns(1);

        _amenityServiceMock
            .CheckAmenityTypeAndUserOwnerShipAsync(1, 1, AmenityType.Hotel, CancellationToken.None)
            .Returns(Result.Success());

        // Act
        var result = await _sut.Handle(Command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_WhenUserIsAdminAndAmenityTypeAndUserOwnershipCheckSucceeds_ShouldCreateAmenity()
    {
        // Arrange
        _userContextMock.Role.Returns(UserRole.Admin);
        _userContextMock.Id.Returns(1);

        _amenityServiceMock
            .CheckAmenityTypeAndUserOwnerShipAsync(1, 1, AmenityType.Hotel, CancellationToken.None)
            .Returns(Result.Success());

        // Act
        await _sut.Handle(Command, CancellationToken.None);

        // Assert
        _amenityRepositoryMock.Received(1).Insert(Arg.Any<Amenity>());
    }

    [Fact]
    public async Task Handle_WhenUserIsAdminAndAmenityTypeAndUserOwnershipCheckSucceeds_ShouldSaveChanges()
    {
        // Arrange
        _userContextMock.Role.Returns(UserRole.Admin);
        _userContextMock.Id.Returns(1);

        _amenityServiceMock
            .CheckAmenityTypeAndUserOwnerShipAsync(1, 1, AmenityType.Hotel, CancellationToken.None)
            .Returns(Result.Success());

        // Act
        await _sut.Handle(Command, CancellationToken.None);

        // Assert
        await _unitOfWorkMock.Received(1).SaveChangesAsync(CancellationToken.None);
    }
}
