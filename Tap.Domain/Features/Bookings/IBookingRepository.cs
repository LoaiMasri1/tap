using Tap.Domain.Core.Primitives.Maybe;

namespace Tap.Domain.Features.Bookings;

public interface IBookingRepository
{
    void Insert(Booking booking);
    Task<Maybe<Booking>> GetByIdAsync(int id, CancellationToken cancellation = default);
    Task<Maybe<Booking>> GetByIdIncludingHotelAndRoomAsync(
        int id,
        CancellationToken cancellation = default
    );
    Task<Maybe<Booking>> GetByIdIncludingHotelAsync(
        int id,
        CancellationToken cancellation = default
    );
    Task<Maybe<Booking>> GetByIdIncludingHotelAndUserAsync(
        int id,
        CancellationToken cancellation = default
    );
    Task<Maybe<Booking>> GetBySessionIdAsync(
        string sessionId,
        CancellationToken cancellationToken = default
    );
}
