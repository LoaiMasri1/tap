using Tap.Application.Core.Abstractions.Notification;
using Tap.Contracts.Emails;
using Tap.Domain.Core.Events;
using Tap.Domain.Features.Bookings.Events;
using Tap.Domain.Features.Hotels;
using Tap.Domain.Features.Users;

namespace Tap.Application.Features.Bookings.Events;

public class BookingCreatedEventHandler : IDomainEventHandler<BookingCreatedEvent>
{
    private readonly IEmailNotificationService _emailNotificationService;
    private readonly IUserRepository _userRepository;
    private readonly IHotelRepository _hotelRepository;

    public BookingCreatedEventHandler(
        IEmailNotificationService emailNotificationService,
        IUserRepository userRepository,
        IHotelRepository hotelRepository
    )
    {
        _emailNotificationService = emailNotificationService;
        _userRepository = userRepository;
        _hotelRepository = hotelRepository;
    }

    public async Task Handle(BookingCreatedEvent notification, CancellationToken cancellationToken)
    {
        var maybeUser = await _userRepository.GetByIdAsync(notification.UserId, cancellationToken);
        var maybeHotel = await _hotelRepository.GetByIdAsync(
            notification.HotelId,
            cancellationToken
        );

        if (maybeUser.HasNoValue || maybeHotel.HasNoValue)
        {
            return;
        }

        var user = maybeUser.Value;
        var hotel = maybeHotel.Value;

        var bookingSuccessEmail = new BookingSuccessEmail(
            user.Name,
            user.Email,
            hotel.Name,
            notification.CheckInDate.ToLongDateString(),
            notification.CheckOutDate.ToLongDateString(),
            notification.TotalPrice,
            notification.Currency
        );

        await _emailNotificationService.SendBookingSuccessEmail(bookingSuccessEmail);
    }
}
