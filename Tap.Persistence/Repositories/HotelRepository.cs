using Tap.Application.Core.Abstractions.Data;
using Tap.Domain.Features.Hotels;

namespace Tap.Persistence.Repositories;

public class HotelRepository : GenericRepository<Hotel>, IHotelRepository
{
    public HotelRepository(IDbContext context)
        : base(context) { }
}
