using Microsoft.Extensions.Configuration;
using Tap.Application.Core.Abstractions.Email;
using Tap.Application.Core.Abstractions.Notification;
using Tap.Contracts.Emails;

namespace Tap.Infrastructure.Notification;

public class EmailNotificationService : IEmailNotificationService
{
    private readonly IEmailService _emailService;
    private readonly IConfiguration _configuration;
    private string BaseUrl => $"{_configuration["Host:Url"]!}/api/v1/";

    public EmailNotificationService(IEmailService emailService, IConfiguration configuration)
    {
        _emailService = emailService;
        _configuration = configuration;
    }

    public async Task SendWelcomeEmail(WelcomeEmail welcomeEmail)
    {
        var body = $"""
                    Welcome to Tap, {welcomeEmail.Name}!

                    Please click the following link to verify your email address: {BaseUrl}auth/activate?t={welcomeEmail.Token}
                    
                    Thank you for using Tap!
                    """;

        var mailRequest = new MailRequest(welcomeEmail.EmailTo, "Welcome to Tap", body);
        await _emailService.SendEmailAsync(mailRequest);
    }

    public async Task SendBookingSuccessEmail(BookingSuccessEmail bookingSuccessEmail)
    {
        var body = $"""
                   Dear {bookingSuccessEmail.Name},
                   
                   Your booking at {bookingSuccessEmail.HotelName} has been successfully created.
                   
                   Please find the booking details below:
                   
                   check in date: {bookingSuccessEmail.CheckInDate}
                   check out date: {bookingSuccessEmail.CheckOutDate}
                   total price: {bookingSuccessEmail.TotalPrice} {bookingSuccessEmail.Currency}
                   
                   Your booking now in pending status. Please wait for the hotel to confirm your booking.
                   
                   Thank you for using Tap!
                   """;

        var mailRequest = new MailRequest(bookingSuccessEmail.EmailTo, "Booking Success", body);

        await _emailService.SendEmailAsync(mailRequest);
    }

    public Task SendBookingConfirmedEmail(BookingConfirmedEmail bookingConfirmedEmail)
    {
        var body = $"""
                    Dear {bookingConfirmedEmail.Name},
                    
                    your booking at {bookingConfirmedEmail.HotelName} has been confirmed.
                    
                    please find the booking details below:
                    check in date: {bookingConfirmedEmail.CheckInDate}
                    check out date: {bookingConfirmedEmail.CheckOutDate}
                    total price: {bookingConfirmedEmail.TotalPrice} {bookingConfirmedEmail.Currency}
                    
                    to checkout, please click the following link: {BaseUrl}bookings/{bookingConfirmedEmail.BookingId}/checkout
                    
                    Thank you for using Tap!
                    """;

        var mailRequest = new MailRequest(bookingConfirmedEmail.EmailTo, "Booking Confirmed", body);

        return _emailService.SendEmailAsync(mailRequest);
    }

    public Task SendBookingCanceledEmail(BookingCanceledEmail bookingCanceledEmail)
    {
        var body = $"""
                    Dear {bookingCanceledEmail.Name},
                    
                    your booking at {bookingCanceledEmail.HotelName} has been canceled.
                    
                    Thank you for using Tap!
                    """;

        var mailRequest = new MailRequest(bookingCanceledEmail.EmailTo, "Booking Canceled", body);

        return _emailService.SendEmailAsync(mailRequest);
    }

    public Task SendBookingPayedEmail(BookingPayedEmail bookingPayedEmail)
    {
        var body = $"""
                    Dear {bookingPayedEmail.Name},
                    
                    Thank you for choosing Tap!
                    your booking {bookingPayedEmail.BookingId} in {bookingPayedEmail.HotelName} has been payed.
                    
                    Total price: {bookingPayedEmail.TotalPrice} {bookingPayedEmail.Currency}
                    
                    Thank you for choosing Tap! , Hope to see you again.
                    
                    """;

        var mailRequest = new MailRequest(bookingPayedEmail.EmailTo, "Booking Payed", body);

        return _emailService.SendEmailAsync(mailRequest);
    }
}
