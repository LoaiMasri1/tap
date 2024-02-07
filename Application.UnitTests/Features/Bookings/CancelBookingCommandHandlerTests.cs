using FluentAssertions;
using NSubstitute;
using Tap.Application.Core.Abstractions.Common;
using Tap.Application.Core.Abstractions.Data;
using Tap.Application.Features.Authentication;
using Tap.Application.Features.Bookings.CancelBooking;
using Tap.Domain.Common.ValueObjects;
using Tap.Domain.Core.Errors;
using Tap.Domain.Core.Primitives.Maybe;
using Tap.Domain.Features.Bookings;
using Tap.Domain.Features.Hotels;
using Tap.Domain.Features.Rooms;
using Tap.Domain.Features.Users;

namespace Application.UnitTests.Features.Bookings;

public class CancelBookingCommandHandlerTests
{
    private readonly IBookingRepository _bookingRepositoryMock;
    private readonly IUserContext _userContextMock;
    private readonly IUnitOfWork _unitOfWorkMock;
    private readonly IDateTime _dateTimeMock;

    private static readonly CancelBookingCommand Command = new(BookingId: 1);

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

    private readonly CancelBookingCommandHandler _sut;

    public CancelBookingCommandHandlerTests()
    {
        _dateTimeMock = Substitute.For<IDateTime>();
        _bookingRepositoryMock = Substitute.For<IBookingRepository>();
        _userContextMock = Substitute.For<IUserContext>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();

        _sut = new CancelBookingCommandHandler(
            _bookingRepositoryMock,
            _dateTimeMock,
            _unitOfWorkMock,
            _userContextMock
        );

        _dateTimeMock.UtcNow.Returns(DateTime.Now);

        _booking = Booking.Create(
            User,
            Hotel,
            Room,
            _dateTimeMock.UtcNow,
            _dateTimeMock.UtcNow.AddDays(1)
        );
    }

    [Fact]
    public async Task Handle_WhenUserIsNotAuthenticated_ShouldReturnUnauthorizedError()
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
    public async Task Handle_WhenBookingDoesNotExist_ShouldReturnBookingNotFoundError()
    {
        // Arrange
        _userContextMock.Role.Returns(UserRole.Admin);
        _bookingRepositoryMock
            .GetByIdIncludingHotelAsync(Command.BookingId, CancellationToken.None)
            .Returns(Maybe<Booking>.None);

        // Act
        var result = await _sut.Handle(Command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.Booking.NotFound);
    }

    // cancel booking success
    [Fact]
    public async Task Handle_WhenBookingExists_ShouldReturnSuccess()
    {
        // Arrange
        _userContextMock.Role.Returns(UserRole.Admin);
        _bookingRepositoryMock
            .GetByIdIncludingHotelAsync(Command.BookingId, CancellationToken.None)
            .Returns(_booking);

        _dateTimeMock.UtcNow.Returns(DateTime.Now.AddDays(-2));

        // Act
        var result = await _sut.Handle(Command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_WhenBookingIsCanceled_ShouldReturnBookingIsCanceledError()
    {
        // Arrange
        _userContextMock.Role.Returns(UserRole.Admin);
        _bookingRepositoryMock
            .GetByIdIncludingHotelAsync(Command.BookingId, CancellationToken.None)
            .Returns(_booking);

        _dateTimeMock.UtcNow.Returns(DateTime.Now.AddDays(-2));

        _booking.Cancel(_dateTimeMock.UtcNow);

        // Act
        var result = await _sut.Handle(Command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.Booking.AlreadyCancelled);
    }

    [Fact]
    public async Task Handle_WhenBookingIsConfirmed_ShouldReturnBookingIsConfirmedError()
    {
        // Arrange
        _userContextMock.Role.Returns(UserRole.Admin);
        _bookingRepositoryMock
            .GetByIdIncludingHotelAsync(Command.BookingId, CancellationToken.None)
            .Returns(_booking);

        _dateTimeMock.UtcNow.Returns(DateTime.Now.AddDays(-2));

        _booking.Confirm();

        // Act
        var result = await _sut.Handle(Command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.Booking.AlreadyConfirmed);
    }

    [Fact]
    public async Task Handle_WhenBookingIsNotCanceled_ShouldReturnBookingIsNotCanceledError()
    {
        // Arrange
        _userContextMock.Role.Returns(UserRole.Admin);
        _bookingRepositoryMock
            .GetByIdIncludingHotelAsync(Command.BookingId, CancellationToken.None)
            .Returns(_booking);

        _dateTimeMock.UtcNow.Returns(DateTime.Now.AddDays(2));

        // Act
        var result = await _sut.Handle(Command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.Booking.CannotCancel);
    }

    [Fact]
    public async Task Handle_WhenBookingIsCanceled_ShouldSaveChanges()
    {
        // Arrange
        _userContextMock.Role.Returns(UserRole.Admin);
        _bookingRepositoryMock
            .GetByIdIncludingHotelAsync(Command.BookingId, CancellationToken.None)
            .Returns(_booking);

        _dateTimeMock.UtcNow.Returns(DateTime.Now.AddDays(-2));

        // Act
        var result = await _sut.Handle(Command, CancellationToken.None);

        // Assert
        await _unitOfWorkMock.Received().SaveChangesAsync(CancellationToken.None);
    }
}
