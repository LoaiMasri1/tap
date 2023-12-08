using Tap.Application.Core.Abstractions.Data;
using Tap.Domain.Features.Users;

namespace Tap.Persistence.Repositories;

internal sealed class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(IDbContext dbContext)
        : base(dbContext) { }

    public Task<bool> IsEmailUniqueAsync(string email, CancellationToken cancellationToken) =>
        AnyAsync(user => user.Email == email, cancellationToken);
}
