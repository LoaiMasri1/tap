namespace Tap.Contracts.Features.Discounts;

public record FilteredDiscountResponse(
    int Id,
    string Name,
    string Description,
    decimal DiscountPercentage
);
