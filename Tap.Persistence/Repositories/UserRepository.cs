using Tap.Application.Core.Abstractions.Data;
using Tap.Domain.Core.Primitives.Maybe;
using Tap.Domain.Features.Users;

namespace Tap.Persistence.Repositories;

internal sealed class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(IDbContext dbContext)
        : base(dbContext) { }

    public Task<bool> IsEmailUniqueAsync(string email, CancellationToken cancellationToken) =>
        AnyAsync(user => user.Email == email, cancellationToken);

    public Task<Maybe<User>> GetByTokenAsync(string token, CancellationToken cancellationToken) => 
        GetByAsync(u => u.ActivationToken.Value == token, cancellationToken);
}
