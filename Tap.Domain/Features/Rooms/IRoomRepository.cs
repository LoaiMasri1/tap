using Tap.Domain.Core.Primitives.Maybe;

namespace Tap.Domain.Features.Rooms;

public interface IRoomRepository
{
    void Insert(Room room);
    Task<Maybe<Room>> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<Maybe<Room>> GetByIdWithDiscountsAsync(int id, CancellationToken cancellationToken);
}
