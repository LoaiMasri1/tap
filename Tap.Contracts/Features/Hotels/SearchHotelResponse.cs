using Tap.Application.Features.Hotels;
using Tap.Contracts.Features.Amenities;
using Tap.Contracts.Features.Rooms;

namespace Tap.Contracts.Features.Hotels;

public record SearchHotelResponse(
    int Id,
    string Name,
    string Description,
    string City,
    int Rating,
    double Longitude,
    double Latitude,
    int NumberOfAvailableRooms,
    FilteredAmenityResponse[] Amenities,
    PhotoResponse[] Photos,
    FilteredRoomResponse[]? Rooms = null
);
