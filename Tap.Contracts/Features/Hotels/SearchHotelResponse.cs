﻿using Tap.Contracts.Features.Amenities;
using Tap.Contracts.Features.Photos;
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
    DateTime Created,
    DateTime? Updated,
    FilteredAmenityResponse[] Amenities,
    PhotoResponse[] Photos,
    FilteredRoomResponse[]? Rooms = null
);
