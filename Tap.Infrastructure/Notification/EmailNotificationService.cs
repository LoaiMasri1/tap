using Tap.Application.Core.Abstractions.Email;
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
        var mailRequest = new MailRequest(
            welcomeEmail.EmailTo,
            "Welcome to Tap",
            $"Welcome to Tap, {welcomeEmail.Name}!"
                + Environment.NewLine
                + Environment.NewLine
                + $"Please click the following link to verify your email address: {BaseUrl}/users/activate?t={welcomeEmail.Token}"
        );
        await _emailService.SendEmailAsync(mailRequest);
    }
}
