namespace Tap.Infrastructure.Authentication.Options;

public class JwtOptions
{
    public const string SectionKey = "Authentication:Schemes:Bearer";

    public string SecretKey { get; set; }
    public string ValidIssuer { get; set; }
    public string ValidAudience { get; set; }
    public int TokenExpirationInMinutes { get; set; }
}
