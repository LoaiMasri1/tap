using Tap.Domain.Core.Events;

namespace Tap.Domain.Features.Bookings.Events;

public record BookingCreatedEvent(
    string GuestName,
    string GuestEmail,
    string HotelName,
    DateTime CheckInDate,
    DateTime CheckOutDate,
    decimal TotalPrice,
    string Currency
) : IDomainEvent;
