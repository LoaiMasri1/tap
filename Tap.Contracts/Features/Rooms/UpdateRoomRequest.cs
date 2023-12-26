using Tap.Domain.Features.Rooms;

namespace Tap.Contracts.Features.Rooms;

public record UpdateRoomRequest(
    int Id,
    int Number,
    decimal Price,
    string Currency,
    RoomType Type,
    int CapacityOfAdults,
    int CapacityOfChildren
);
