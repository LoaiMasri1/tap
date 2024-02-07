using FluentAssertions;
using NSubstitute;
using Tap.Application.Core.Abstractions.Data;
using Tap.Application.Features.Authentication;
using Tap.Application.Features.Hotels.UpdateHotel;
using Tap.Domain.Core.Errors;
using Tap.Domain.Core.Primitives.Maybe;
using Tap.Domain.Features.Hotels;
using Tap.Domain.Features.Users;

namespace Application.UnitTests.Features.Hotels;

public class UpdateHotelCommandTests
{
    private readonly IHotelRepository _hotelRepositoryMock;
    private readonly IUserContext _userContextMock;
    private readonly IUnitOfWork _unitOfWorkMock;

    private static readonly UpdateHotelCommand Command =
        new(1, "Hotel Name Old", "Hotel Description Old", 35.0, 35.0);

    private static readonly Hotel Hotel =
        new(
            "Hotel Name Old",
            "Hotel Description Old",
            Location.Create(35.0, 35.0).Value,
            new User("User Name", Email.From("test@example.com"), "Password", UserRole.User)
        );

    private readonly UpdateHotelCommandHandler _sut;

    public UpdateHotelCommandTests()
    {
        _hotelRepositoryMock = Substitute.For<IHotelRepository>();
        _userContextMock = Substitute.For<IUserContext>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();

        _sut = new UpdateHotelCommandHandler(
            _hotelRepositoryMock,
            _unitOfWorkMock,
            _userContextMock
        );
    }

    [Fact]
    public async Task Handle_WhenHotelDoesNotExist_ReturnsNotFoundError()
    {
        _userContextMock.Role.Returns(UserRole.Admin);
        _hotelRepositoryMock
            .GetByIdAsync(Command.Id, CancellationToken.None)
            .Returns(Maybe<Hotel>.None);

        var result = await _sut.Handle(Command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.Hotel.NotFound);
    }

    [Fact]
    public async Task Handle_WhenUserIsNotTheHotelOwner_ReturnsUnauthorizedError()
    {
        _userContextMock.Id.Returns(2);
        _hotelRepositoryMock.GetByIdAsync(Command.Id, CancellationToken.None).Returns(Hotel);

        var result = await _sut.Handle(Command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.User.Unauthorized);
    }

    [Fact]
    public async Task Handle_WhenHotelExistsAndNewLocationIsInvalid_ReturnsInvalidLocationError()
    {
        _userContextMock.Id.Returns(Hotel.UserId);
        _hotelRepositoryMock.GetByIdAsync(Command.Id, CancellationToken.None).Returns(Hotel);

        var result = await _sut.Handle(Command with { Latitude = 100.0 }, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.Location.LatitudeOutOfRange);
    }

    [Fact]
    public async Task Handle_WhenUserProvideSameData_ReturnsNothingToUpdateError()
    {
        _userContextMock.Role.Returns(UserRole.Admin);
        _hotelRepositoryMock.GetByIdAsync(Command.Id, CancellationToken.None).Returns(Hotel);
        Hotel.Update(
            Command.Name,
            Command.Description,
            Location.Create(Command.Latitude, Command.Longitude).Value
        );

        var result = await _sut.Handle(Command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.Hotel.NothingToUpdate);
    }

    [Fact]
    public async Task Handle_WhenHotelExistsAndNewLocationIsValid_ReturnsSuccessResult()
    {
        _userContextMock.Id.Returns(Hotel.UserId);
        _hotelRepositoryMock.GetByIdAsync(Command.Id, CancellationToken.None).Returns(Hotel);

        var result = await _sut.Handle(Command with { Latitude = 36.0 }, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_WhenHotelExistsAndNewLocationIsValid_ShouldCallUnitOfWorkSaveChanges()
    {
        _userContextMock.Id.Returns(Hotel.UserId);
        _hotelRepositoryMock.GetByIdAsync(Command.Id, CancellationToken.None).Returns(Hotel);

        Hotel.Update(
            Command.Name,
            Command.Description,
            Location.Create(Command.Latitude, Command.Longitude).Value
        );

        await _sut.Handle(Command with { Latitude = 36.0 }, CancellationToken.None);

        await _unitOfWorkMock.Received(1).SaveChangesAsync(CancellationToken.None);
    }
}
