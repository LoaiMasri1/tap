using Tap.Application.Core.Abstractions.Data;
using Tap.Domain.Features.Bookings;

namespace Tap.Persistence.Repositories;

public class BookingRepository : GenericRepository<Booking>, IBookingRepository
{
    public BookingRepository(IDbContext dbContext)
        : base(dbContext) { }
}
