using FluentAssertions;
using NSubstitute;
using Tap.Application.Core.Abstractions.Data;
using Tap.Application.Features.Authentication;
using Tap.Application.Features.Bookings.ConfirmBooking;
using Tap.Domain.Common.ValueObjects;
using Tap.Domain.Core.Errors;
using Tap.Domain.Core.Primitives.Maybe;
using Tap.Domain.Features.Bookings;
using Tap.Domain.Features.Hotels;
using Tap.Domain.Features.Rooms;
using Tap.Domain.Features.Users;

namespace Application.UnitTests.Features.Bookings;

public class ConfirmBookingCommandHandlerTests
{
    private readonly IBookingRepository _bookingRepositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;
    private readonly IUserContext _userContextMock;

    private static readonly ConfirmBookingCommand Command = new(BookingId: 1);

    private readonly ConfirmBookingCommandHandler _sut;

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

    private readonly Booking _booking;

    public ConfirmBookingCommandHandlerTests()
    {
        _bookingRepositoryMock = Substitute.For<IBookingRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();
        _userContextMock = Substitute.For<IUserContext>();

        _sut = new ConfirmBookingCommandHandler(
            _bookingRepositoryMock,
            _unitOfWorkMock,
            _userContextMock
        );

        _booking = Booking.Create(User, Hotel, Room, DateTime.Now, DateTime.Now.AddDays(1));
    }

    [Fact]
    public async Task Handle_WhenUserIsNotAuthorized_ReturnsUnauthorizedError()
    {
        _userContextMock.Role.Returns(UserRole.User);

        var result = await _sut.Handle(Command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.User.Unauthorized);
    }

    [Fact]
    public async Task Handle_WhenUserIsNotTheOwnerOfTheHotel_ShouldReturnUnauthorizedError()
    {
        // Arrange
        _userContextMock.Role.Returns(UserRole.Admin);
        _bookingRepositoryMock
            .GetByIdIncludingHotelAsync(Command.BookingId, CancellationToken.None)
            .Returns(_booking);

        _userContextMock.Id.Returns(2);

        // Act
        var result = await _sut.Handle(Command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.User.Unauthorized);
    }

    [Fact]
    public async Task Handle_WhenBookingIsNotFound_ReturnsBookingNotFoundError()
    {
        _userContextMock.Role.Returns(UserRole.Admin);
        _bookingRepositoryMock
            .GetByIdIncludingHotelAsync(Command.BookingId, CancellationToken.None)
            .Returns(Maybe<Booking>.None);

        var result = await _sut.Handle(Command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.Booking.NotFound);
    }

    [Fact]
    public async Task Handle_WhenBookingIsNotPending_ReturnsBookingIsNotPendingError()
    {
        _userContextMock.Role.Returns(UserRole.Admin);
        _bookingRepositoryMock
            .GetByIdIncludingHotelAsync(Command.BookingId, CancellationToken.None)
            .Returns(_booking);

        _booking.Confirm();

        var result = await _sut.Handle(Command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.Booking.IsNotPending);
    }

    [Fact]
    public async Task Handle_WhenBookingIsPending_ReturnsSuccess()
    {
        _userContextMock.Role.Returns(UserRole.Admin);
        _bookingRepositoryMock
            .GetByIdIncludingHotelAsync(Command.BookingId, CancellationToken.None)
            .Returns(_booking);

        var result = await _sut.Handle(Command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_WhenBookingIsPending_SavesChanges()
    {
        _userContextMock.Role.Returns(UserRole.Admin);
        _bookingRepositoryMock
            .GetByIdIncludingHotelAsync(Command.BookingId, CancellationToken.None)
            .Returns(_booking);

        await _sut.Handle(Command, CancellationToken.None);

        await _unitOfWorkMock.Received(1).SaveChangesAsync(CancellationToken.None);
    }
}
