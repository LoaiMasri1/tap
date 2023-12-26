namespace Tap.Contracts.Features.Discounts;

public record CreateDiscountRequest(
    int RoomId,
    string Name,
    string Description,
    decimal DiscountPercentage,
    DateTime StartDate,
    DateTime EndDate
);
