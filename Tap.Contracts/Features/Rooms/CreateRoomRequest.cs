using Tap.Domain.Features.Rooms;

namespace Tap.Contracts.Features.Rooms;

public record CreateRoomRequest(
    int HotelId,
    int Number,
    decimal Price,
    string Currency,
    RoomType Type,
    int CapacityOfAdults,
    int CapacityOfChildren
);
