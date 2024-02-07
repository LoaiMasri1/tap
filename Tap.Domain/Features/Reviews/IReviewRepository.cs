using Tap.Domain.Core.Primitives.Maybe;

namespace Tap.Domain.Features.Reviews;

public interface IReviewRepository
{
    void Insert(Review review);
    void Remove(Review review);
    Task<Maybe<Review>> GetByIdAsync(int id, CancellationToken cancellationToken);
}
