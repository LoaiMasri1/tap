using Tap.Application.Core.Messaging;
using Tap.Contracts.Features.Bookings;
using Tap.Domain.Core.Primitives.Result;

namespace Tap.Application.Features.Bookings.BookRoom;

public record BookRoomCommand(int RoomId, DateTime CheckInDate, DateTime CheckOutDate)
    : ICommand<Result<BookingResponse>>;
