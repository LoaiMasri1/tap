using Tap.Application.Core.Messaging;
using Tap.Contracts.Features.Reviews;
using Tap.Domain.Core.Primitives.Result;

namespace Tap.Application.Features.Reviews.CreateReview;

public record CreateReviewCommand(string Title, string Content, int Rating, int HotelId, int UserId)
    : ICommand<Result<ReviewResponse>>;
