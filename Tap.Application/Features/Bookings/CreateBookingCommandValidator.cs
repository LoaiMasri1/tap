﻿using FluentValidation;
using Tap.Application.Core.Errors;
using Tap.Application.Core.Extensions;

namespace Tap.Application.Features.Bookings;

public class CreateBookingCommandValidator : AbstractValidator<BookRoomCommand>
{
    public CreateBookingCommandValidator()
    {
        RuleFor(x => x.RoomId).NotEmpty().WithError(ValidationErrors.CreateBooking.RoomIdRequired);
        RuleFor(x => x.CheckInDate)
            .NotEmpty()
            .WithError(ValidationErrors.CreateBooking.CheckInDateRequired);
        RuleFor(x => x.CheckOutDate)
            .NotEmpty()
            .WithError(ValidationErrors.CreateBooking.CheckOutDateRequired)
            .GreaterThan(x => x.CheckInDate)
            .WithError(ValidationErrors.CreateBooking.CheckOutDateMustBeGreaterThanCheckInDate);
    }
}
