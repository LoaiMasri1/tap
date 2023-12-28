namespace Tap.Domain.Features.Reviews;

public interface IReviewRepository
{
    void Insert(Review review);
    void Remove(Review review);
}
