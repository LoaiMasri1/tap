namespace Tap.Domain.Features.Users;

public interface IUserRepository
{
    Task<bool> IsEmailUniqueAsync(Email email);
    void Insert(User user);
}
