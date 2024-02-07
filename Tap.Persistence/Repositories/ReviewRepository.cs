using Tap.Application.Core.Abstractions.Data;
using Tap.Domain.Features.Reviews;

namespace Tap.Persistence.Repositories;

public class ReviewRepository : GenericRepository<Review>, IReviewRepository
{
    public ReviewRepository(IDbContext dbContext)
        : base(dbContext) { }
}
