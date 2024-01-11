using FluentAssertions;
using NSubstitute;
using Tap.Application.Core.Abstractions.Data;
using Tap.Application.Features.Authentication;
using Tap.Application.Features.Rooms.BookRoom;
using Tap.Domain.Common.ValueObjects;
using Tap.Domain.Core.Errors;
using Tap.Domain.Core.Primitives.Maybe;
using Tap.Domain.Features.Bookings;
using Tap.Domain.Features.Hotels;
using Tap.Domain.Features.Rooms;
using Tap.Domain.Features.Users;

namespace Application.UnitTests.Features.Rooms;

public class BookRoomCommandTests
{
    private readonly IUserRepository _userRepositoryMock;
    private readonly IRoomRepository _roomRepositoryMock;
    private readonly IUserContext _userContextMock;
    private readonly IUnitOfWork _unitOfWorkMock;

    private static readonly BookRoomCommand Command =
        new(RoomId: 1, DateTime.Now, DateTime.Now.AddDays(1));

    private static readonly User User =
        new("Test Name", Email.From("test@example.com"), "Test Password", UserRole.User);

    private static readonly Room Room = Room.Create(
        1,
        Money.Create(500, "usd"),
        RoomType.Cabana,
        1,
        1
    );

    private static readonly Hotel Hotel =
        new("Test Hotel", "Test Description", Location.Create(35.5, 35.5).Value, User);

    private static readonly Booking Booking = Booking.Create(
        User,
        Hotel,
        Room,
        DateTime.Now,
        DateTime.Now.AddDays(1)
    );

    private readonly BookRoomCommandHandler _sut;

    public BookRoomCommandTests()
    {
        _userRepositoryMock = Substitute.For<IUserRepository>();
        _roomRepositoryMock = Substitute.For<IRoomRepository>();
        _userContextMock = Substitute.For<IUserContext>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();

        _sut = new BookRoomCommandHandler(
            _userRepositoryMock,
            _userContextMock,
            _unitOfWorkMock,
            _roomRepositoryMock
        );
    }

    [Fact]
    public async Task Handle_WhenUserIsNotUser_ReturnsUnauthorizedError()
    {
        _userContextMock.Role.Returns(UserRole.Admin);

        var result = await _sut.Handle(Command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.User.Unauthorized);
    }

    [Fact]
    public async Task Handle_WhenUserNotFound_ReturnUserNotFoundError()
    {
        _userContextMock.Role.Returns(UserRole.User);
        _userRepositoryMock
            .GetByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(Maybe<User>.None);

        var result = await _sut.Handle(Command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.User.NotFound);
    }

    [Fact]
    public async Task Handle_WhenRoomNotFound_ReturnRoomNotFoundError()
    {
        _userContextMock.Role.Returns(UserRole.User);
        _userRepositoryMock
            .GetByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(User);
        _roomRepositoryMock
            .GetByIdWithHotelAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(Maybe<Room>.None);

        var result = await _sut.Handle(Command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.Room.NotFound);
    }
}
