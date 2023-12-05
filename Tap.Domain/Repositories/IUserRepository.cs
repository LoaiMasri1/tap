using Tap.Domain.Entities;

namespace Tap.Domain.Repositories;

public interface IUserRepository
{
    Task<bool> IsEmailUniqueAsync(string email, CancellationToken cancellationToken);
    void Insert(User user);
}
