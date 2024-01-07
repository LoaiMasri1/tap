using Microsoft.EntityFrameworkCore;
using Tap.Application.Core.Abstractions.Data;
using Tap.Domain.Core.Primitives.Maybe;
using Tap.Domain.Features.Rooms;

namespace Tap.Persistence.Repositories;

public class RoomRepository : GenericRepository<Room>, IRoomRepository
{
    public RoomRepository(IDbContext dbContext)
        : base(dbContext) { }

    public async Task<Maybe<Room>> GetByIdWithDiscountsAsync(
        int id,
        CancellationToken cancellationToken
    )
    {
        var room = await DbContext
            .Set<Room>()
            .Include(r => r.Discounts)
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

        return room is null ? Maybe<Room>.None : Maybe<Room>.From(room);
    }

    public async Task<Maybe<Room>> GetByIdWithHotelAsync(
        int id,
        CancellationToken cancellationToken
    )
    {
        var room = await DbContext
            .Set<Room>()
            .Include(r => r.Discounts)
            .Include(r => r.Hotel)
            .ThenInclude(h => h.Bookings)
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

        return room is null ? Maybe<Room>.None : Maybe<Room>.From(room);
    }
}
