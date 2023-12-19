using Tap.Application.Core.Abstractions.Data;
using Tap.Domain.Core.Primitives.Maybe;
using Tap.Domain.Features.Cities;

namespace Tap.Persistence.Repositories;

public class CityRepository : GenericRepository<City>, ICityRepository
{
    public CityRepository(IDbContext context)
        : base(context) { }

    public async Task<Maybe<City>> GetByNameAsync(
        string name,
        CancellationToken cancellationToken
    ) => await GetByAsync(city => city.Name == name, cancellationToken);
}
