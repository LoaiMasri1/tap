using Tap.Application.Core.Abstractions.Data;
using Tap.Application.Core.Messaging;
using Tap.Application.Features.Authentication;
using Tap.Contracts.Features.Reviews;
using Tap.Domain.Core.Errors;
using Tap.Domain.Core.Primitives.Result;
using Tap.Domain.Features.Reviews;
using Tap.Domain.Features.Users;

namespace Tap.Application.Features.Reviews.UpdateReview;

public class UpdateReviewCommandHandler
    : ICommandHandler<UpdateReviewCommand, Result<ReviewResponse>>
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;

    public UpdateReviewCommandHandler(
        IReviewRepository reviewRepository,
        IUnitOfWork unitOfWork,
        IUserContext userContext
    )
    {
        _reviewRepository = reviewRepository;
        _unitOfWork = unitOfWork;
        _userContext = userContext;
    }

    public async Task<Result<ReviewResponse>> Handle(
        UpdateReviewCommand request,
        CancellationToken cancellationToken
    )
    {
        if (_userContext.Role == UserRole.Admin)
        {
            return DomainErrors.User.Unauthorized;
        }

        var userId = _userContext.Id;

        var maybeReview = await _reviewRepository.GetByIdAsync(request.Id, cancellationToken);

        if (maybeReview.HasNoValue)
        {
            return DomainErrors.Review.NotFound;
        }

        var review = maybeReview.Value;

        if (review.HotelId is null || review.UserId is null)
        {
            return DomainErrors.General.UnProcessableRequest;
        }

        if (review.UserId != userId)
        {
            return DomainErrors.User.Unauthorized;
        }

        var result = review.Update(request.Title, request.Content, request.Rating);

        if (result.IsFailure)
        {
            return result.Error;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new ReviewResponse(
            review.Id,
            review.Title,
            review.Content,
            review.Rating,
            review.HotelId.Value,
            review.UserId.Value
        );
    }
}
