using Tap.Domain.Core.Primitives.Maybe;

namespace Tap.Domain.Features.Cities;

public interface ICityRepository
{
    void Insert(City city);
    Task<Maybe<City>> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<Maybe<City>> GetByNameAsync(string name, CancellationToken cancellationToken);
}
