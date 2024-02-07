using FluentValidation;
using Tap.Application.Core.Errors;
using Tap.Application.Core.Extensions;

namespace Tap.Application.Features.Bookings.CancelBooking;

public class CancelBookingCommandValidator : AbstractValidator<CancelBookingCommand>
{
    public CancelBookingCommandValidator()
    {
        RuleFor(x => x.BookingId)
            .GreaterThan(0)
            .NotEmpty()
            .WithError(ValidationErrors.CancelBooking.BookingIdIsRequired);
    }
}
