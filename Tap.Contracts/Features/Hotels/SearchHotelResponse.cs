using Tap.Application.Features.Hotels;
using Tap.Contracts.Features.Amenities;

namespace Tap.Contracts.Features.Hotels;

public record SearchHotelResponse(
    int Id,
    string Name,
    string City,
    int Rating,
    double Longitude,
    double Latitude,
    int NumberOfAvailableRooms,
    FilteredAmenityResponse[] Amenities,
    PhotoResponse[] Photos
);
