namespace Tap.Contracts.Features.Discounts;

public record CreateDiscountRequest(
    int RoomId,
    string Name,
    string Description,
    int DiscountPercentage,
    DateTime StartDate,
    DateTime EndDate
);
