using FluentAssertions;
using NSubstitute;
using Tap.Application.Core.Abstractions.Data;
using Tap.Application.Features.Authentication;
using Tap.Application.Features.Rooms.CreateRoom;
using Tap.Contracts.Features.Rooms;
using Tap.Domain.Core.Errors;
using Tap.Domain.Core.Primitives.Maybe;
using Tap.Domain.Features.Hotels;
using Tap.Domain.Features.Rooms;
using Tap.Domain.Features.Users;

namespace Application.UnitTests.Features.Rooms;

public class CreateRoomCommandTests
{
    private readonly IHotelRepository _hotelRepositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;
    private readonly IUserIdentifierProvider _userIdentifierProviderMock;

    private static readonly CreateRoomCommand Command =
        new(1, 1, 100, "USD", RoomType.Single, 1, 0);

    private readonly CreateRoomCommandHandler _sut;

    public CreateRoomCommandTests()
    {
        _hotelRepositoryMock = Substitute.For<IHotelRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();
        _userIdentifierProviderMock = Substitute.For<IUserIdentifierProvider>();

        _sut = new CreateRoomCommandHandler(
            _hotelRepositoryMock,
            _unitOfWorkMock,
            _userIdentifierProviderMock
        );
    }

    [Fact]
    public async Task Handle_WhenUserIsNotAdmin_ReturnsUnauthorizedError()
    {
        _userIdentifierProviderMock.Role.Returns(UserRole.User);

        var result = await _sut.Handle(Command, CancellationToken.None);

        result.Error.Should().Be(DomainErrors.User.Unauthorized);
    }

    [Fact]
    public async Task Handle_WhenHotelDoesNotExist_ReturnsHotelNotFoundError()
    {
        _userIdentifierProviderMock.Role.Returns(UserRole.Admin);
        _hotelRepositoryMock
            .GetByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(Maybe<Hotel>.None);

        var result = await _sut.Handle(Command, CancellationToken.None);

        result.Error.Should().Be(DomainErrors.Hotel.NotFound);
    }

    [Fact]
    public async Task Handle_WhenHotelExists_ReturnsRoomResponse()
    {
        _userIdentifierProviderMock.Role.Returns(UserRole.Admin);
        _hotelRepositoryMock
            .GetByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(
                new Hotel(
                    "Name",
                    "Description",
                    Location.Create(35, 35).Value,
                    new User(
                        "FirstName",
                        Email.From("test@example.com"),
                        "Password",
                        UserRole.Admin
                    )
                )
            );

        var result = await _sut.Handle(Command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeOfType<RoomResponse>();
    }

    [Fact]
    public async Task Handle_WhenHotelExists_AddsRoomToHotel()
    {
        _userIdentifierProviderMock.Role.Returns(UserRole.Admin);
        _hotelRepositoryMock
            .GetByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(
                new Hotel(
                    "Name",
                    "Description",
                    Location.Create(35, 35).Value,
                    new User(
                        "FirstName",
                        Email.From("test@example.com"),
                        "Password",
                        UserRole.Admin
                    )
                )
            );

        var result = await _sut.Handle(Command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeOfType<RoomResponse>();
    }

    [Fact]
    public async Task Handle_WhenHotelExists_SavesChanges()
    {
        _userIdentifierProviderMock.Role.Returns(UserRole.Admin);
        _hotelRepositoryMock
            .GetByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(
                new Hotel(
                    "Name",
                    "Description",
                    Location.Create(35, 35).Value,
                    new User(
                        "FirstName",
                        Email.From("test@example.com"),
                        "Password",
                        UserRole.Admin
                    )
                )
            );

        var result = await _sut.Handle(Command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeOfType<RoomResponse>();

        await _unitOfWorkMock.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
