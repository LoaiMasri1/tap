using Tap.Application.Core.Abstractions.Notification;
using Tap.Contracts.Emails;
using Tap.Domain.Core.Events;
using Tap.Domain.Features.Bookings.Events;

namespace Tap.Application.Features.Bookings.Events;

public class BookingCreatedEventHandler : IDomainEventHandler<BookingCreatedEvent>
{
    private readonly IEmailNotificationService _emailNotificationService;

    public BookingCreatedEventHandler(IEmailNotificationService emailNotificationService)
    {
        _emailNotificationService = emailNotificationService;
    }

    public async Task Handle(BookingCreatedEvent notification, CancellationToken cancellationToken)
    {
        var bookingSuccessEmail = new BookingSuccessEmail(
            notification.GuestName,
            notification.GuestEmail,
            notification.HotelName,
            notification.CheckInDate.ToLongDateString(),
            notification.CheckOutDate.ToLongDateString(),
            notification.TotalPrice,
            notification.Currency
        );

        await _emailNotificationService.SendBookingSuccessEmail(bookingSuccessEmail);
    }
}
