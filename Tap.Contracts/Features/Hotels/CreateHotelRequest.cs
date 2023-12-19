namespace Tap.Contracts.Features.Hotels;

public record CreateHotelRequest(
    string Name,
    string Description,
    double Latitude,
    double Longitude,
    int CityId
);
