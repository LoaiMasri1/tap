using Tap.Domain.Core.Events;

namespace Tap.Application.Features.Reviews.DeleteReview.Events;

public class ReviewDeletedEvent : IDomainEvent
{
    public int? HotelId { get; private set; }

    public ReviewDeletedEvent(int? hotelId)
    {
        HotelId = hotelId;
    }
}
