using Tap.Domain.Common.ValueObjects;
using Tap.Domain.Features.Rooms;

namespace Tap.Contracts.Features.Rooms;

public record RoomResponse(
    int Id,
    int Number,
    Money Price,
    decimal DiscountedPrice,
    RoomType Type,
    bool IsAvailable,
    int CapacityOfAdults,
    int CapacityOfChildren
);
