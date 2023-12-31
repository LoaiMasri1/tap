using Tap.Application.Core.Messaging;
using Tap.Contracts.Features.Reviews;
using Tap.Domain.Core.Primitives.Result;

namespace Tap.Application.Features.Reviews.UpdateReview;

public record UpdateReviewCommand(int Id, string Title, string Content, int Rating)
    : ICommand<Result<ReviewResponse>>;
