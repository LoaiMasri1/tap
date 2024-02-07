using FluentAssertions;
using NSubstitute;
using Tap.Application.Core.Abstractions.Data;
using Tap.Application.Core.Abstractions.Sessions;
using Tap.Application.Features.Authentication;
using Tap.Application.Features.Bookings.CheckoutBooking;
using Tap.Domain.Common;
using Tap.Domain.Common.ValueObjects;
using Tap.Domain.Core.Errors;
using Tap.Domain.Core.Primitives.Maybe;
using Tap.Domain.Features.Bookings;
using Tap.Domain.Features.Hotels;
using Tap.Domain.Features.Rooms;
using Tap.Domain.Features.Users;

namespace Application.UnitTests.Features.Bookings;

public class CheckoutBookingCommandHandlerTests
{
    private readonly IBookingRepository _bookingRepositoryMock;
    private readonly ISessionService _sessionServiceMock;
    private readonly IUserContext _userContextMock;
    private readonly IUnitOfWork _unitOfWorkMock;

    private static readonly CheckoutBookingCommand Command = new(BookingId: 1);

    private static readonly User User =
        new("Test Name", Email.From("test@example.com"), "Test Password", UserRole.User);

    private static readonly Hotel Hotel =
        new("Test Hotel", "Test Description", Location.Create(35.5, 35.5).Value, User);

    private static readonly Room Room = Room.Create(
        1,
        Money.Create(500, "usd"),
        RoomType.Cabana,
        1,
        1
    );

    private static readonly Booking Booking = Booking.Create(
        User,
        Hotel,
        Room,
        DateTime.Now,
        DateTime.Now.AddDays(1)
    );

    private readonly CheckoutBookingCommandHandler _sut;

    public CheckoutBookingCommandHandlerTests()
    {
        _bookingRepositoryMock = Substitute.For<IBookingRepository>();
        _sessionServiceMock = Substitute.For<ISessionService>();
        _userContextMock = Substitute.For<IUserContext>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();

        _sut = new CheckoutBookingCommandHandler(
            _bookingRepositoryMock,
            _sessionServiceMock,
            _userContextMock,
            _unitOfWorkMock
        );
    }

    [Fact]
    public async Task Handle_WhenUserIsNotAuthenticated_ReturnsUnauthorizedError()
    {
        _userContextMock.Role.Returns(UserRole.Admin);

        var result = await _sut.Handle(Command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.User.Unauthorized);
    }

    [Fact]
    public async Task Handle_WhenBookingDoesNotExist_ReturnsNotFoundError()
    {
        _userContextMock.Role.Returns(UserRole.User);
        _bookingRepositoryMock
            .GetByIdAsync(Command.BookingId, Arg.Any<CancellationToken>())
            .Returns(Maybe<Booking>.None);

        var result = await _sut.Handle(Command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.Booking.NotFound);
    }

    [Fact]
    public async Task Handle_WhenBookingDoesNotBelongToUser_ReturnsUnauthorizedError()
    {
        _userContextMock.Role.Returns(UserRole.User);
        _bookingRepositoryMock
            .GetByIdAsync(Command.BookingId, Arg.Any<CancellationToken>())
            .Returns(Booking);

        _userContextMock.Id.Returns(2);

        var result = await _sut.Handle(Command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.User.Unauthorized);
    }

    [Fact]
    public async Task Handle_WhenSessionDoesNotExist_ReturnsNotFoundError()
    {
        _userContextMock.Role.Returns(UserRole.User);
        _bookingRepositoryMock
            .GetByIdAsync(Command.BookingId, Arg.Any<CancellationToken>())
            .Returns(Booking);

        _sessionServiceMock
            .SaveAsync(Command.BookingId, Arg.Any<CancellationToken>())
            .Returns(Maybe<Session>.None);

        var result = await _sut.Handle(Command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.Session.NotFound);
    }

    [Fact]
    public async Task Handle_WhenSessionExists_ReturnsSession()
    {
        _userContextMock.Role.Returns(UserRole.User);
        _bookingRepositoryMock
            .GetByIdAsync(Command.BookingId, Arg.Any<CancellationToken>())
            .Returns(Booking);

        _sessionServiceMock
            .SaveAsync(Command.BookingId, Arg.Any<CancellationToken>())
            .Returns(new Session("test_session_id", "test_publishable_key"));

        var result = await _sut.Handle(Command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeOfType<Session>();
    }

    [Fact]
    public async Task Handle_WhenSessionExists_CallsSaveChangesAsyncOnce()
    {
        _userContextMock.Role.Returns(UserRole.User);
        _bookingRepositoryMock
            .GetByIdAsync(Command.BookingId, Arg.Any<CancellationToken>())
            .Returns(Booking);

        _sessionServiceMock
            .SaveAsync(Command.BookingId, Arg.Any<CancellationToken>())
            .Returns(new Session("test_session_id", "test_publishable_key"));

        await _sut.Handle(Command, CancellationToken.None);

        await _unitOfWorkMock.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
