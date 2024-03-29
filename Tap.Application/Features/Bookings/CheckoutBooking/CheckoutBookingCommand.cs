﻿using Tap.Application.Core.Messaging;
using Tap.Domain.Common;
using Tap.Domain.Core.Primitives.Result;

namespace Tap.Application.Features.Bookings.CheckoutBooking;

public record CheckoutBookingCommand(int BookingId) : ICommand<Result<Session>>;
