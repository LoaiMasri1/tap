using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Tap.Application.Features.Authentication;
using Tap.Domain.Features.Users;

namespace Tap.Infrastructure.Authentication;

public class UserContext : IUserContext
{
    public UserContext(IHttpContextAccessor httpContextAccessor)
    {
        Id = int.Parse(
            httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)!.Value
                ?? throw new ArgumentException(
                    "The user identifier claim is required.",
                    nameof(httpContextAccessor)
                )
        );

        Role = Enum.Parse<UserRole>(
            httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Role)!.Value
                ?? throw new ArgumentException(
                    "The user role claim is required.",
                    nameof(httpContextAccessor)
                )
        );

        Email =
            httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Email)!.Value
            ?? throw new ArgumentException(
                "The user email claim is required.",
                nameof(httpContextAccessor)
            );
    }

    public int Id { get; }
    public UserRole Role { get; }
    public string Email { get; }
}
