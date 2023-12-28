using FluentValidation;
using Tap.Application.Core.Errors;
using Tap.Application.Core.Extensions;

namespace Tap.Application.Features.Reviews.UpdateReview;

public class UpdateReviewCommandValidator : AbstractValidator<UpdateReviewCommand>
{
    public UpdateReviewCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithError(ValidationErrors.UpdateReview.ReviewIdIsRequired);
        RuleFor(x => x.Title).NotEmpty().WithError(ValidationErrors.UpdateReview.TitleIsRequired);
        RuleFor(x => x.Content)
            .NotEmpty()
            .WithError(ValidationErrors.UpdateReview.ContentIsRequired);
        RuleFor(x => x.Rating).NotEmpty().WithError(ValidationErrors.UpdateReview.RatingIsRequired);
    }
}
