using FluentValidation;
using Tap.Application.Core.Errors;
using Tap.Application.Core.Extensions;

namespace Tap.Application.Features.Reviews.DeleteReview;

public class DeleteReviewCommandValidator : AbstractValidator<DeleteReviewCommand>
{
    public DeleteReviewCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty()
            .GreaterThan(0)
            .WithError(ValidationErrors.DeleteReview.ReviewIdIsRequired);
    }
}
