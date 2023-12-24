using Tap.Application.Core.Abstractions.Data;
using Tap.Domain.Features.Amenities;

namespace Tap.Persistence.Repositories;

public class AmenityRepository : GenericRepository<Amenity>, IAmenityRepository
{
    public AmenityRepository(IDbContext dbContext)
        : base(dbContext) { }
}
