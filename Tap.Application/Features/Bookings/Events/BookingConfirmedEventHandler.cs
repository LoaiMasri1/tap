using Tap.Application.Core.Abstractions.Notification;
using Tap.Contracts.Emails;
using Tap.Domain.Core.Events;
using Tap.Domain.Features.Bookings;
using Tap.Domain.Features.Bookings.Events;

namespace Tap.Application.Features.Bookings.Events;

public class BookingConfirmedEventHandler : IDomainEventHandler<BookingConfirmedEvent>
{
    private readonly IEmailNotificationService _emailNotificationService;
    private readonly IBookingRepository _bookingRepository;

    public BookingConfirmedEventHandler(
        IEmailNotificationService emailNotificationService,
        IBookingRepository bookingRepository
    )
    {
        _emailNotificationService = emailNotificationService;
        _bookingRepository = bookingRepository;
    }

    public async Task Handle(
        BookingConfirmedEvent notification,
        CancellationToken cancellationToken
    )
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

        var bookingConfirmedEmail = new BookingConfirmedEmail(
            booking.Id,
            booking.User.Name,
            booking.User.Email,
            booking.Hotel.Name,
            booking.CheckInDate.ToLongDateString(),
            booking.CheckOutDate.ToLongDateString(),
            booking.TotalPrice,
            booking.Room.Price.Currency
        );

        Console.WriteLine(
            $"Sending booking confirmed email to {booking.User.Email} for booking {booking.Id}"
        );

        await _emailNotificationService.SendBookingConfirmedEmail(bookingConfirmedEmail);
    }
}
