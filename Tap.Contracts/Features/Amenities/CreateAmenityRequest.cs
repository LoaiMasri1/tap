namespace Tap.Contracts.Features.Amenities;

public record CreateAmenityRequest(string Name, string Description, int TypeId);
