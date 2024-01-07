using FluentValidation;
using Tap.Application.Core.Errors;
using Tap.Application.Core.Extensions;
using Tap.Application.Features.Bookings.ConfirmBooking;

namespace Tap.Application.Features.Bookings.ConfirmBook;

public class ConfirmBookingCommandValidator : AbstractValidator<ConfirmBookingCommand>
{
    public ConfirmBookingCommandValidator()
    {
        RuleFor(x => x.BookingId)
            .NotEmpty()
            .WithError(ValidationErrors.ConfirmBook.BookingIdRequired);
    }
}
