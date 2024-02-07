namespace Tap.Contracts.Features.Reviews;

public record ReviewResponse(
    int Id,
    string Title,
    string Content,
    int Rating,
    int HotelId,
    int UserId
);
