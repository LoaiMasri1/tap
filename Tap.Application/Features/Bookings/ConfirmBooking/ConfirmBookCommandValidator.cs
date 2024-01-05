using FluentValidation;
using Tap.Application.Core.Errors;
using Tap.Application.Core.Extensions;
using Tap.Application.Features.Bookings.ConfirmBooking;

namespace Tap.Application.Features.Bookings.ConfirmBook;

public class ConfirmBookCommandValidator : AbstractValidator<ConfirmBookingCommand>
{
    public ConfirmBookCommandValidator()
    {
        RuleFor(x => x.BookingId)
            .NotEmpty()
            .WithError(ValidationErrors.ConfirmBook.BookingIdRequired);
    }
}
