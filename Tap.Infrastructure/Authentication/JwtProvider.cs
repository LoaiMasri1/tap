using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Tap.Application.Core.Abstractions.Common;
using Tap.Application.Features.Authentication;
using Tap.Domain.Features.Users;
using Tap.Infrastructure.Authentication.Options;

namespace Tap.Infrastructure.Authentication;

public class JwtProvider : IJwtProvider
{
    private readonly JwtOptions _jwtOptions;
    private readonly IDateTime _dateTime;

    public JwtProvider(IOptions<JwtOptions> jwtOptions, IDateTime dateTime)
    {
        _dateTime = dateTime;
        _jwtOptions = jwtOptions.Value;
    }

    public string Create(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Name),
            new(ClaimTypes.Email, user.Email.Value),
            new(ClaimTypes.Role, user.Role.ToString())
        };

        var tokenExpirationDate = _dateTime.UtcNow.AddMinutes(_jwtOptions.TokenExpirationInMinutes);

        var token = new JwtSecurityToken(
            _jwtOptions.ValidIssuer,
            _jwtOptions.ValidAudience,
            claims,
            expires: tokenExpirationDate,
            signingCredentials: credentials
        );

        var tokenHandler = new JwtSecurityTokenHandler();

        return tokenHandler.WriteToken(token);
    }
}
