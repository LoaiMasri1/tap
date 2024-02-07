namespace Tap.Contracts.Features.Reviews;

public record CreateReviewRequest(
    string Title,
    string Content,
    int Rating,
    int HotelId,
    int UserId
);
