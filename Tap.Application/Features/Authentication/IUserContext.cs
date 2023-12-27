using Tap.Domain.Features.Users;

namespace Tap.Application.Features.Authentication;

public interface IUserContext
{
    int Id { get; }
    UserRole Role { get; }
}
