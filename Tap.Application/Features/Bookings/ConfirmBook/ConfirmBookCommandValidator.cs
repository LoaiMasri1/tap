using FluentValidation;
using Tap.Application.Core.Errors;
using Tap.Application.Core.Extensions;

namespace Tap.Application.Features.Bookings.ConfirmBook;

public class ConfirmBookCommandValidator : AbstractValidator<ConfirmBookCommand>
{
    public ConfirmBookCommandValidator()
    {
        RuleFor(x => x.BookingId)
            .NotEmpty()
            .WithError(ValidationErrors.ConfirmBook.BookingIdRequired);
    }
}
