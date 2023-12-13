using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using Tap.Application.Core.Abstractions.Email;
using Tap.Contracts.Emails;
using Tap.Infrastructure.Emails.Options;

namespace Tap.Infrastructure.Emails;

public class EmailService : IEmailService
{
    private readonly MailOptions _mailOptions;

    public EmailService(IOptions<MailOptions> mailOptions) => _mailOptions = mailOptions.Value;

    public async Task SendEmailAsync(MailRequest mailRequest)
    {
        var email = new MimeMessage
        {
            From = { new MailboxAddress(_mailOptions.SenderDisplayName, _mailOptions.SenderEmail) },
            To = { MailboxAddress.Parse(mailRequest.EmailTo) },
            Subject = mailRequest.Subject,
            Body = new TextPart(TextFormat.Text) { Text = mailRequest.Body }
        };

        using var smtpClient = new SmtpClient();

        await smtpClient.ConnectAsync(
            _mailOptions.SmtpServer,
            _mailOptions.SmtpPort,
            SecureSocketOptions.StartTls
        );

        await smtpClient.AuthenticateAsync(_mailOptions.SenderEmail, _mailOptions.SmtpPassword);

        await smtpClient.SendAsync(email);

        await smtpClient.DisconnectAsync(true);
    }
}
