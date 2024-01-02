using Tap.Domain.Core.Primitives.Maybe;

namespace Tap.Domain.Features.Bookings;

public interface IBookingRepository
{
    void Insert(Booking booking);
    Task<Maybe<Booking>> GetByIdAsync(int id, CancellationToken cancellation = default);
}
