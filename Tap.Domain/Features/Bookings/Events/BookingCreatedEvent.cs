using Tap.Domain.Core.Events;

namespace Tap.Domain.Features.Bookings.Events;

public record BookingCreatedEvent(
    int UserId,
    int HotelId,
    DateTime CheckInDate,
    DateTime CheckOutDate,
    decimal TotalPrice,
    string Currency
) : IDomainEvent;
