using Tap.Contracts.Emails;

namespace Tap.Application.Core.Abstractions.Email;

public interface IEmailService
{
    Task SendEmailAsync(MailRequest mailRequest);
}
