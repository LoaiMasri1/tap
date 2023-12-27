using FluentAssertions;
using NSubstitute;
using Tap.Application.Core.Abstractions.Data;
using Tap.Application.Features.Amenities.UpdateAmenity;
using Tap.Application.Features.Authentication;
using Tap.Contracts.Features.Amenities;
using Tap.Domain.Core.Errors;
using Tap.Domain.Core.Primitives.Maybe;
using Tap.Domain.Core.Primitives.Result;
using Tap.Domain.Features.Amenities;
using Tap.Domain.Features.Users;

namespace Application.UnitTests.Features.Amenities;

public class UpdateAmenityCommandTests
{
    private readonly IAmenityRepository _amenityRepositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;
    private readonly IUserContext _userContextMock;

    private static readonly UpdateAmenityCommand Command =
        new(1, "Amenity Name", "Amenity Description");
    private readonly UpdateAmenityCommandHandler _sut;

    public UpdateAmenityCommandTests()
    {
        _amenityRepositoryMock = Substitute.For<IAmenityRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();
        _userContextMock = Substitute.For<IUserContext>();

        _sut = new UpdateAmenityCommandHandler(
            _unitOfWorkMock,
            _amenityRepositoryMock,
            _userContextMock
        );
    }

    [Fact]
    public async Task Handle_WhenUserIsNotAdmin_ReturnsUnauthorizedError()
    {
        _userContextMock.Role.Returns(UserRole.User);

        var result = await _sut.Handle(Command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.User.Unauthorized);
    }

    [Fact]
    public async Task Handle_WhenAmenityDoesNotExist_ReturnsNotFoundError()
    {
        _userContextMock.Role.Returns(UserRole.Admin);
        _amenityRepositoryMock
            .GetByIdAsync(Command.Id, CancellationToken.None)
            .Returns(Maybe<Amenity>.None);

        var result = await _sut.Handle(Command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.Amenity.NotFound);
    }

    [Theory]
    [InlineData(AmenityType.Hotel)]
    [InlineData(AmenityType.Room)]
    public async Task Handle_WhenAmenityExists_ReturnsUpdatedAmenityResponse(AmenityType type)
    {
        _userContextMock.Role.Returns(UserRole.Admin);
        _amenityRepositoryMock
            .GetByIdAsync(Command.Id, CancellationToken.None)
            .Returns(Amenity.Create("Old Name", "Old Description", type, 1));

        var result = await _sut.Handle(Command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeOfType<AmenityResponse>();
    }

    [Theory]
    [InlineData(AmenityType.Hotel)]
    [InlineData(AmenityType.Room)]
    public async Task Handle_WhenAmenityExists_ReturnsUpdatedAmenityResponseWithSameType(
        AmenityType type
    )
    {
        _userContextMock.Role.Returns(UserRole.Admin);

        var amenity = Amenity.Create("Old Name", "Old Description", type, 1);

        _amenityRepositoryMock.GetByIdAsync(Command.Id, CancellationToken.None).Returns(amenity);

        amenity.Update(Command.Name, Command.Description);

        var result = await _sut.Handle(Command, CancellationToken.None);

        result.Error.Should().Be(DomainErrors.Amenity.NothingToUpdate);
    }
}
