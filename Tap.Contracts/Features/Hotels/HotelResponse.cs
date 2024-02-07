namespace Tap.Contracts.Features.Hotels;

public record HotelResponse(
    int Id,
    string Name,
    string Description,
    double Latitude,
    double Longitude
);
