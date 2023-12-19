using Tap.Domain.Features.Users;

namespace Tap.Application.Features.Authentication;

public interface IUserIdentifierProvider
{
    int Id { get; }
    UserRole Role { get; }
}
