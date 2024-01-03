using Tap.Domain.Core.Events;

namespace Tap.Domain.Features.Bookings.Events;

public record BookingConfirmedEvent(int BookingId) : IDomainEvent;
