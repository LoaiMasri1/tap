using FluentValidation;
using Tap.Application.Core.Errors;
using Tap.Application.Core.Extensions;

namespace Tap.Application.Features.Reviews.CreateReview;

public class CreateReviewCommandValidator : AbstractValidator<CreateReviewCommand>
{
    public CreateReviewCommandValidator()
    {
        RuleFor(x => x.Title).NotEmpty().WithError(ValidationErrors.CreateReview.TitleIsRequired);
        RuleFor(x => x.Content)
            .NotEmpty()
            .WithError(ValidationErrors.CreateReview.ContentIsRequired);
        RuleFor(x => x.Rating)
            .NotEmpty()
            .GreaterThanOrEqualTo(1)
            .WithError(ValidationErrors.CreateReview.RatingIsRequired);

        RuleFor(x => x.HotelId)
            .NotEmpty()
            .GreaterThan(0)
            .WithError(ValidationErrors.CreateReview.HotelIdIsRequired);
        RuleFor(x => x.UserId)
            .NotEmpty()
            .GreaterThan(0)
            .WithError(ValidationErrors.CreateReview.UserIdIsRequired);
    }
}
