using Tap.Application.Core.Messaging;
using Tap.Domain.Core.Primitives.Result;

namespace Tap.Application.Features.Bookings.ConfirmBook;

public record ConfirmBookCommand(int BookingId) : ICommand<Result>;
