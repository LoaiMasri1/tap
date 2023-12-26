using Tap.Application.Core.Abstractions.Data;
using Tap.Domain.Features.Rooms;

namespace Tap.Persistence.Repositories;

public class RoomRepository : GenericRepository<Room>, IRoomRepository
{
    public RoomRepository(IDbContext dbContext)
        : base(dbContext) { }
}
