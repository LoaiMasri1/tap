namespace Tap.Contracts.Features.Discounts;

public record DiscountResponse(int Id, string Name, string Description, decimal DiscountPercentage);
