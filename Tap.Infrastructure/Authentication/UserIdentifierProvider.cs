using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Tap.Application.Features.Authentication;

namespace Tap.Infrastructure.Authentication;

public class UserIdentifierProvider : IUserIdentifierProvider
{
    public UserIdentifierProvider(IHttpContextAccessor httpContextAccessor)
    {
        Id = int.Parse(
            httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)!.Value
                ?? throw new ArgumentException(
                    "The user identifier claim is required.",
                    nameof(httpContextAccessor)
                )
        );
    }

    public int Id { get; }
}
