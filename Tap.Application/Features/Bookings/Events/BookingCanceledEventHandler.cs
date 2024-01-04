using Tap.Application.Core.Abstractions.Notification;
using Tap.Contracts.Emails;
using Tap.Domain.Core.Events;
using Tap.Domain.Features.Bookings;
using Tap.Domain.Features.Bookings.Events;

namespace Tap.Application.Features.Bookings.Events;

public class BookingCanceledEventHandler : IDomainEventHandler<BookingCanceledEvent>
{
    private readonly IEmailNotificationService _emailNotificationService;
    private readonly IBookingRepository _bookingRepository;

    public BookingCanceledEventHandler(
        IEmailNotificationService emailNotificationService,
        IBookingRepository bookingRepository
    )
    {
        _emailNotificationService = emailNotificationService;
        _bookingRepository = bookingRepository;
    }

    public async Task Handle(BookingCanceledEvent notification, CancellationToken cancellationToken)
    {
        var maybeBooking = await _bookingRepository.GetByIdIncludingHotelAndRoomAsync(
            notification.BookingId,
            cancellationToken
        );

        if (maybeBooking.HasNoValue)
        {
            return;
        }

        var booking = maybeBooking.Value;

        var result = booking.Room.UpdateAvailability(true);

        if (result.IsFailure)
        {
            return;
        }

        await _emailNotificationService.SendBookingCanceledEmail(
            new BookingCanceledEmail(booking.User.Name, booking.User.Email, booking.Hotel.Name)
        );
    }
}
