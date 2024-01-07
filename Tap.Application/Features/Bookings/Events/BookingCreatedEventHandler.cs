using Tap.Application.Core.Abstractions.Common;
using Tap.Application.Core.Abstractions.Notification;
using Tap.Contracts.Emails;
using Tap.Domain.Core.Events;
using Tap.Domain.Features.Bookings.Events;

namespace Tap.Application.Features.Bookings.Events;

public class BookingCreatedEventHandler : IDomainEventHandler<BookingCreatedEvent>
{
    private readonly IEmailNotificationService _emailNotificationService;
    private readonly IDateTime _dateTime;

    public BookingCreatedEventHandler(
        IEmailNotificationService emailNotificationService,
        IDateTime dateTime
    )
    {
        _emailNotificationService = emailNotificationService;
        _dateTime = dateTime;
    }

    public async Task Handle(BookingCreatedEvent notification, CancellationToken cancellationToken)
    {
        var bookingSuccessEmail = new BookingSuccessEmail(
            notification.GuestName,
            notification.GuestEmail,
            notification.HotelName,
            _dateTime.ToLongDateString(notification.CheckInDate),
            _dateTime.ToLongDateString(notification.CheckOutDate),
            notification.TotalPrice,
            notification.Currency
        );

        await _emailNotificationService.SendBookingSuccessEmail(bookingSuccessEmail);
    }
}
