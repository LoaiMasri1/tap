using Tap.Application.Core.Abstractions.Data;
using Tap.Application.Core.Messaging;
using Tap.Application.Features.Authentication;
using Tap.Domain.Core.Errors;
using Tap.Domain.Core.Primitives.Result;
using Tap.Domain.Features.Reviews;
using Tap.Domain.Features.Reviews.Events;

namespace Tap.Application.Features.Reviews.DeleteReview
{
    public class DeleteReviewCommandHandler : ICommandHandler<DeleteReviewCommand, Result>
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IUserContext _userContext;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteReviewCommandHandler(
            IReviewRepository reviewRepository,
            IUserContext userContext,
            IUnitOfWork unitOfWork
        )
        {
            _reviewRepository = reviewRepository;
            _userContext = userContext;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(
            DeleteReviewCommand command,
            CancellationToken cancellationToken
        )
        {
            var userId = _userContext.Id;

            var maybeReview = await _reviewRepository.GetByIdAsync(command.Id, cancellationToken);

            if (maybeReview.HasNoValue)
            {
                return DomainErrors.Review.NotFound;
            }

            var review = maybeReview.Value;

            if (review.UserId != userId)
            {
                return DomainErrors.User.Unauthorized;
            }

            _reviewRepository.Remove(review);

            review.AddDomainEvent(new ReviewDeletedEvent(review.HotelId));

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
