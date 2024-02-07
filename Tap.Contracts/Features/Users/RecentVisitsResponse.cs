namespace Tap.Contracts.Features.Users;

public record RecentVisitsResponse(
    string HotelName,
    string? ImageUrl,
    string CityName,
    int Rating,
    decimal Price,
    string CheckInDate,
    string CheckOutDate
);
