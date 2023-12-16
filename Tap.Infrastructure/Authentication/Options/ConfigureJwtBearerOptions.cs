using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Tap.Infrastructure.Authentication.Options;

public class ConfigureJwtBearerOptions : IConfigureNamedOptions<JwtBearerOptions>
{
    private readonly JwtOptions _jwtOptions;

    public ConfigureJwtBearerOptions(IOptions<JwtOptions> options) => _jwtOptions = options.Value;

    public void Configure(JwtBearerOptions options)
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = _jwtOptions.ValidIssuer,
            ValidAudience = _jwtOptions.ValidAudience,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_jwtOptions.SecretKey)
            )
        };
    }

    public void Configure(string? name, JwtBearerOptions options) => Configure(options);
}
