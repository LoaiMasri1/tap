namespace Tap.Contracts.Emails;

public record WelcomeEmail(string Name, string EmailTo, string Token);
