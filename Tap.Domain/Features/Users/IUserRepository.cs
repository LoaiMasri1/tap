using Tap.Domain.Core.Primitives.Maybe;

namespace Tap.Domain.Features.Users;

public interface IUserRepository
{
    Task<bool> IsEmailUniqueAsync(string email, CancellationToken cancellationToken);
    void Insert(User user);
    Task<Maybe<User>> GetByTokenAsync(string token, CancellationToken cancellationToken);
    Task<Maybe<User>> GetByEmailAsync(string email, CancellationToken cancellationToken);
    Task<Maybe<User>> GetByIdAsync(int id, CancellationToken cancellationToken);
}
