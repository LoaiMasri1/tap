using Tap.Domain.Core.Events;

namespace Tap.Domain.Features.Reviews.Events;

public class ReviewUpdatedEvent : IDomainEvent
{
    public int? HotelId { get; private set; }

    public ReviewUpdatedEvent(int? hotelId)
    {
        HotelId = hotelId;
    }
}
