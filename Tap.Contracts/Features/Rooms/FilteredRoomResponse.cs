using Tap.Contracts.Features.Amenities;
using Tap.Contracts.Features.Photos;
using Tap.Domain.Features.Rooms;

namespace Tap.Contracts.Features.Rooms;

public record FilteredRoomResponse(
    int Id,
    int RoomNumber,
    RoomType RoomType,
    int CapacityOfAdults,
    int CapacityOfChildren,
    int DiscountPercentage,
    decimal PricePerNight,
    decimal DiscountedPricePerNight,
    string Currency,
    bool IsAvailable,
    FilteredAmenityResponse[] Amenities,
    PhotoResponse[] Photos,
    DateTime Created,
    DateTime? Updated
);
