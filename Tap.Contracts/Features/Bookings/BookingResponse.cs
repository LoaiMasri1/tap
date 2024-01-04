namespace Tap.Contracts.Features.Bookings;

public record BookingResponse(
    int Id,
    int HotelId,
    int RoomId,
    int UserId,
    DateTime CheckInDate,
    DateTime CheckOutDate,
    decimal TotalPrice
);
