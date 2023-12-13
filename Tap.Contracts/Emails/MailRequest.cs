namespace Tap.Contracts.Emails;

public record MailRequest(string EmailTo, string Subject, string Body);
