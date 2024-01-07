using FluentValidation;
using Tap.Application.Core.Errors;
using Tap.Application.Core.Extensions;

namespace Tap.Application.Features.Bookings.ConfirmBooking;

public class ConfirmBookingCommandValidator : AbstractValidator<ConfirmBookingCommand>
{
    public ConfirmBookingCommandValidator()
    {
        RuleFor(x => x.BookingId)
            .NotEmpty()
            .WithError(ValidationErrors.ConfirmBook.BookingIdRequired);
    }
}
