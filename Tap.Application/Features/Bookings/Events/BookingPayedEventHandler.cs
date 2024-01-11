using Tap.Application.Core.Abstractions.Notification;
using Tap.Contracts.Emails;
using Tap.Domain.Core.Events;
using Tap.Domain.Features.Bookings;
using Tap.Domain.Features.Bookings.Events;

namespace Tap.Application.Features.Bookings.Events;

public class BookingPayedEventHandler : IDomainEventHandler<BookingPayedEvent>
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IEmailNotificationService _emailNotificationService;

    public BookingPayedEventHandler(
        IEmailNotificationService emailNotificationService,
        IBookingRepository bookingRepository
    )
    {
        _emailNotificationService = emailNotificationService;
        _bookingRepository = bookingRepository;
    }

    public async Task Handle(BookingPayedEvent notification, CancellationToken cancellationToken)
    {
        var maybeBooking = await _bookingRepository.GetByIdIncludingHotelAndRoomAsync(
            notification.BookingId,
            cancellationToken
        );

        if (maybeBooking.HasNoValue)
            return;

        var booking = maybeBooking.Value;

        var email = new BookingPayedEmail(
            booking.Id,
            booking.User.Name,
            booking.User.Email,
            booking.Hotel.Name,
            booking.TotalPrice,
            booking.Room.Price.Currency
        );

        await _emailNotificationService.SendBookingPayedEmail(email);
    }
}
