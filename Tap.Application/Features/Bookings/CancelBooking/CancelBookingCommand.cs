using Tap.Application.Core.Messaging;
using Tap.Domain.Core.Primitives.Result;

namespace Tap.Application.Features.Bookings.CancelBooking;

public record CancelBookingCommand(int BookingId) : ICommand<Result>;
