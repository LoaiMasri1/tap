using Tap.Application.Core.Messaging;
using Tap.Domain.Core.Primitives.Result;

namespace Tap.Application.Features.Reviews.DeleteReview;

public record DeleteReviewCommand(int Id) : ICommand<Result>;
