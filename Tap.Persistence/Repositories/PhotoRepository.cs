using Tap.Application.Core.Abstractions.Data;
using Tap.Domain.Core.Utility;
using Tap.Domain.Features.Photos;

namespace Tap.Persistence.Repositories;

public class PhotoRepository : GenericRepository<Photo>, IPhotoRepository
{
    public PhotoRepository(IDbContext dbContext)
        : base(dbContext) { }
}
