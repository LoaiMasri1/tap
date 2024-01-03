﻿using Tap.Application.Core.Abstractions.Email;
using Tap.Application.Core.Abstractions.Notification;
using Tap.Contracts.Emails;

namespace Tap.Infrastructure.Notification;

public class EmailNotificationService : IEmailNotificationService
{
    private readonly IEmailService _emailService;
    private const string BaseUrl = "https://localhost:5001";

    public EmailNotificationService(IEmailService emailService)
    {
        _emailService = emailService;
    }

    public async Task SendWelcomeEmail(WelcomeEmail welcomeEmail)
    {
        var body = $"""
                    Welcome to Tap, {welcomeEmail.Name}!

                    Please click the following link to verify your email address: {BaseUrl}/users/activate?t={welcomeEmail.Token}
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
}
