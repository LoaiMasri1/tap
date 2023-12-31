using Microsoft.EntityFrameworkCore;
using Tap.Application.Core.Abstractions.Data;
using Tap.Domain.Core.Primitives.Maybe;
using Tap.Domain.Features.Hotels;

namespace Tap.Persistence.Repositories;

public class HotelRepository : GenericRepository<Hotel>, IHotelRepository
{
    public HotelRepository(IDbContext context)
        : base(context) { }

    public async Task<Maybe<Hotel>> GetHotelIncludeRatingsAsync(
        int id,
        CancellationToken cancellationToken
    )
    {
        var hotel = await DbContext
            .Set<Hotel>()
            .Include(h => h.Reviews)
            .SingleOrDefaultAsync(h => h.Id == id, cancellationToken);

        return hotel is null ? Maybe<Hotel>.None : Maybe<Hotel>.From(hotel);
    }
}
