namespace Tap.Domain.Features.Users;

public interface IUserRepository
{
    Task<bool> IsEmailUniqueAsync(string email, CancellationToken cancellationToken);
    void Insert(User user);
}
