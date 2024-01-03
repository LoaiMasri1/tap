using Tap.Contracts.Emails;

namespace Tap.Application.Core.Abstractions.Notification
{
    public interface IEmailNotificationService
    {
        Task SendWelcomeEmail(WelcomeEmail welcomeEmail);
        Task SendBookingSuccessEmail(BookingSuccessEmail bookingSuccessEmail);
        Task SendBookingConfirmedEmail(BookingConfirmedEmail bookingConfirmedEmail);
        Task SendBookingCanceledEmail(BookingCanceledEmail bookingCanceledEmail);
    }
}
