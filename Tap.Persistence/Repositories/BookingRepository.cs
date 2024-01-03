using Microsoft.EntityFrameworkCore;
using Tap.Application.Core.Abstractions.Data;
using Tap.Domain.Core.Primitives.Maybe;
using Tap.Domain.Features.Bookings;

namespace Tap.Persistence.Repositories;

public class BookingRepository : GenericRepository<Booking>, IBookingRepository
{
    public BookingRepository(IDbContext dbContext)
        : base(dbContext) { }

    public async Task<Maybe<Booking>> GetByIdIncludingHotelAndRoomAsync(
        int id,
        CancellationToken cancellation = default
    )
    {
        var booking = await DbContext
            .Set<Booking>()
            .Include(b => b.User)
            .Include(b => b.Room)
            .Include(b => b.Hotel)
            .FirstOrDefaultAsync(b => b.Id == id, cancellation);

        return booking is null ? Maybe<Booking>.None : Maybe<Booking>.From(booking);
    }

    public async Task<Maybe<Booking>> GetByIdIncludingHotelAsync(
        int id,
        CancellationToken cancellation = default
    )
    {
        var booking = await DbContext
            .Set<Booking>()
            .Include(b => b.Hotel)
            .FirstOrDefaultAsync(b => b.Id == id, cancellation);

        return booking is null ? Maybe<Booking>.None : Maybe<Booking>.From(booking);
    }

    public async Task<Maybe<Booking>> GetByIdIncludingHotelAndUserAsync(
        int id,
        CancellationToken cancellation = default
    )
    {
        var booking = await DbContext
            .Set<Booking>()
            .Include(b => b.User)
            .Include(b => b.Hotel)
            .FirstOrDefaultAsync(b => b.Id == id, cancellation);

        return booking is null ? Maybe<Booking>.None : Maybe<Booking>.From(booking);
    }
}
