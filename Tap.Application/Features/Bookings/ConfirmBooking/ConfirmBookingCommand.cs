using Tap.Application.Core.Messaging;
using Tap.Domain.Core.Primitives.Result;

namespace Tap.Application.Features.Bookings.ConfirmBooking;

public record ConfirmBookingCommand(int BookingId) : ICommand<Result>;
