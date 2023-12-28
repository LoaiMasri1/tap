using Tap.Domain.Core.Events;
using Tap.Domain.Features.Hotels;

namespace Tap.Application.Features.Reviews.DeleteReview.Events;

public class ReviewDeletedEventHandler : IDomainEventHandler<ReviewDeletedEvent>
{
    private readonly IHotelRepository _hotelRepository;

    public ReviewDeletedEventHandler(IHotelRepository hotelRepository)
    {
        _hotelRepository = hotelRepository;
    }

    public async Task Handle(ReviewDeletedEvent notification, CancellationToken cancellationToken)
    {
        if (notification.HotelId is null)
        {
            return;
        }

        var maybeHotel = await _hotelRepository.GetHotelIncludeRatingsAsync(
            notification.HotelId.Value,
            cancellationToken
        );

        if (maybeHotel.HasNoValue)
        {
            return;
        }

        var hotel = maybeHotel.Value;

        hotel.UpdateRating();
    }
}
