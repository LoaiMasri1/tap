using Tap.Domain.Features.Users;

namespace Tap.Application.Features.Authentication;

public interface IJwtProvider
{
    string Create(User user);
}
