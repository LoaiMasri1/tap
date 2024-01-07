using Tap.Domain.Features.Bookings;

namespace Tap.Contracts.Features.Bookings;

public record BookingResponse(
    int Id,
    int HotelId,
    int RoomId,
    int UserId,
    BookingStatus Status,
    DateTime CheckInDate,
    DateTime CheckOutDate,
    decimal TotalPrice
);
