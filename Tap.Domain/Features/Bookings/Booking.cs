using Tap.Domain.Core.Abstraction;
using Tap.Domain.Core.Errors;
using Tap.Domain.Core.Primitives;
using Tap.Domain.Core.Primitives.Result;
using Tap.Domain.Core.Utility;
using Tap.Domain.Features.Bookings.Events;
using Tap.Domain.Features.Hotels;
using Tap.Domain.Features.Rooms;
using Tap.Domain.Features.Users;

namespace Tap.Domain.Features.Bookings;

public class Booking : AggregateRoot, IAuditableEntity
{
    public DateTime CheckInDate { get; private set; }
    public DateTime CheckOutDate { get; private set; }
    public decimal TotalPrice { get; private set; }
    public BookingStatus Status { get; private set; }
    public User User { get; private set; }
    public int UserId { get; private set; }
    public Room Room { get; private set; }
    public int RoomId { get; private set; }
    public Hotel Hotel { get; private set; }
    public int HotelId { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime? UpdatedAtUtc { get; private set; }

    private Booking() { }

    private Booking(User user, Hotel hotel, Room room, DateTime checkInDate, DateTime checkOutDate)
    {
        Ensure.NotNull(user, "The user is required.", nameof(user));
        Ensure.NotNull(hotel, "The hotel is required.", nameof(hotel));
        Ensure.NotNull(room, "The room is required.", nameof(room));
        Ensure.NotDefault(checkInDate, "The check in date is required.", nameof(checkInDate));
        Ensure.NotDefault(checkOutDate, "The check out date is required.", nameof(checkOutDate));

        User = user;
        UserId = user.Id;
        Hotel = hotel;
        HotelId = hotel.Id;
        Room = room;
        RoomId = room.Id;
        CheckInDate = checkInDate;
        CheckOutDate = checkOutDate;
        Status = BookingStatus.Pending;

        CalculateTotalPrice(room.DiscountedPrice);

        AddDomainEvent(
            new BookingCreatedEvent(
                user.Name,
                user.Email,
                hotel.Name,
                CheckInDate,
                CheckOutDate,
                TotalPrice,
                room.Price.Currency
            )
        );
    }

    public static Booking Create(
        User user,
        Hotel hotel,
        Room room,
        DateTime checkInDate,
        DateTime checkOutDate
    )
    {
        return new Booking(user, hotel, room, checkInDate, checkOutDate);
    }

    public void CalculateTotalPrice(decimal pricePerNight)
    {
        Ensure.NotDefault(pricePerNight, "The price per night is required.", nameof(pricePerNight));

        TotalPrice = pricePerNight * (decimal)CheckOutDate.Subtract(CheckInDate).TotalDays;
    }

    public Result Confirm() =>
        Result
            .Create(Status)
            .Ensure(x => x == BookingStatus.Confirmed, DomainErrors.Booking.AlreadyConfirmed)
            .Tap(_ => Status = BookingStatus.Confirmed)
            .Tap(_ => AddDomainEvent(new BookingConfirmedEvent(Id)));

    public Result Cancel(DateTime now)
    {
        if (Status == BookingStatus.Cancelled)
        {
            return DomainErrors.Booking.AlreadyCancelled;
        }

        if (Status == BookingStatus.Confirmed)
        {
            return DomainErrors.Booking.AlreadyConfirmed;
        }

        if (CheckInDate < now.AddDays(1))
        {
            return DomainErrors.Booking.CannotCancel;
        }

        Status = BookingStatus.Cancelled;

        AddDomainEvent(new BookingCanceledEvent(Id));

        return Result.Success();
    }

    public Result Update(DateTime checkInDate, DateTime checkOutDate)
    {
        Ensure.NotDefault(checkInDate, "The check in date is required.", nameof(checkInDate));
        Ensure.NotDefault(checkOutDate, "The check out date is required.", nameof(checkOutDate));

        if (CheckInDate == checkInDate && CheckOutDate == checkOutDate)
        {
            return DomainErrors.Booking.NothingToUpdate;
        }

        CheckInDate = checkInDate;
        CheckOutDate = checkOutDate;

        CalculateTotalPrice(Room.DiscountedPrice);

        return Result.Success();
    }
}
