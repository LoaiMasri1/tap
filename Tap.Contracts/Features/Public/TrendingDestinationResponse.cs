namespace Tap.Contracts.Features.Public;

public record TrendingDestinationResponse(
    string Name,
    string? ImageUrl,
    string Description,
    string CityName,
    string CountryName
);
