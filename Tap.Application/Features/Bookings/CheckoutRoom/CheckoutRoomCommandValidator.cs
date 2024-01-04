using FluentValidation;
using Tap.Application.Core.Errors;
using Tap.Application.Core.Extensions;

namespace Tap.Application.Features.Bookings.CheckoutRoom;

public class CheckoutBookingCommandValidator : AbstractValidator<CheckoutBookingCommand>
{
    public CheckoutBookingCommandValidator()
    {
        RuleFor(x => x.BookingId)
            .NotEmpty()
            .WithError(ValidationErrors.CheckoutRoom.BookingIdIsRequired);
    }
}
