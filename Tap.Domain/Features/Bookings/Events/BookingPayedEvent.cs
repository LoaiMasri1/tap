using Tap.Domain.Core.Events;

namespace Tap.Domain.Features.Bookings.Events;

public class BookingPayedEvent : IDomainEvent
{
    public int BookingId { get; }

    public BookingPayedEvent(int bookingId)
    {
        BookingId = bookingId;
    }
}
