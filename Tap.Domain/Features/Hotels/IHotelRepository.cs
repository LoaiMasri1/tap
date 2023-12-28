using Tap.Domain.Core.Primitives.Maybe;

namespace Tap.Domain.Features.Hotels;

public interface IHotelRepository
{
    void Insert(Hotel hotel);
    Task<Maybe<Hotel>> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<Maybe<Hotel>> GetHotelIncludeRatingsAsync(int id, CancellationToken cancellationToken);
}
