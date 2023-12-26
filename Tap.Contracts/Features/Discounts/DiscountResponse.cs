namespace Tap.Contracts.Features.Discounts;

public record DiscountResponse(
    int Id,
    decimal DiscountPercentage,
    DateTime StartDate,
    DateTime EndDate
);
