using System.Linq.Expressions;
using Tap.Domain.Core.Primitives.Maybe;

namespace Tap.Domain.Features.Amenities;

public interface IAmenityRepository
{
    void Insert(Amenity amenity);

    Task<Maybe<Amenity>> GetByAsync(
        Expression<Func<Amenity, bool>> predicate,
        CancellationToken cancellationToken = default
    );

    Task<Maybe<Amenity>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    void Remove(Amenity amenity);
}
