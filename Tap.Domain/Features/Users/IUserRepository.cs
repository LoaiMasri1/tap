namespace Tap.Domain.Features.Users;

public interface IUserRepository
{
    Task<bool> IsEmailUniqueAsync(Email email, CancellationToken cancellationToken);
    void Insert(User user);
}
