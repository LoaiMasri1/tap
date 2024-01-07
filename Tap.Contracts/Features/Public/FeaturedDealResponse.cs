namespace Tap.Contracts.Features.Public;

public record FeaturedDealResponse(
    string HotelName,
    string? ImageUrl,
    double Latitude,
    double Longitude,
    string CityName,
    int Rating,
    decimal? Price,
    int? Discount,
    decimal? DiscountedPrice
);
