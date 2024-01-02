using Tap.Application.Features.Hotels;
using Tap.Contracts.Features.Amenities;
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
    PhotoResponse[] Photos
);
