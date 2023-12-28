using Tap.Domain.Core.Events;
using Tap.Domain.Features.Hotels;
using Tap.Domain.Features.Reviews.Events;

namespace Tap.Application.Features.Reviews.Events;

public class ReviewUpdatedEventHandler : IDomainEventHandler<ReviewUpdatedEvent>
{
    private readonly IHotelRepository _hotelRepository;

    public ReviewUpdatedEventHandler(IHotelRepository hotelRepository)
    {
        _hotelRepository = hotelRepository;
    }

    public async Task Handle(ReviewUpdatedEvent notification, CancellationToken cancellationToken)
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
