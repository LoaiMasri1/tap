using Tap.Application.Core.Messaging;
using Tap.Contracts.Features.Rooms;
using Tap.Domain.Core.Primitives.Result;
using Tap.Domain.Features.Rooms;

namespace Tap.Application.Features.Rooms.CreateRoom;

public record CreateRoomCommand(
    int HotelId,
    int Number,
    decimal Price,
    string Currency,
    RoomType Type,
    int CapacityOfAdults,
    int CapacityOfChildren
) : ICommand<Result<RoomResponse>>;
