namespace Tap.Infrastructure.Payment.Options;

public class StripeOptions
{
    public const string SectionName = "Stripe";
    public string SecretKey { get; set; }
    public string PublishableKey { get; set; }
    public string CancelUrl { get; set; }
    public string SuccessUrl { get; set; }
    public string Mode { get; set; }
    public List<string> PaymentMethods { get; set; }
}
