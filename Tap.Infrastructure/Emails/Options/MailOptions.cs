namespace Tap.Infrastructure.Emails.Options;

public class MailOptions
{
    public const string SectionName = "Mail";
    public string SenderDisplayName { get; set; }
    public string SenderEmail { get; set; }
    public string SmtpPassword { get; set; }
    public string SmtpServer { get; set; }
    public int SmtpPort { get; set; }
}
