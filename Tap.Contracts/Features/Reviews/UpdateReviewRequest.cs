namespace Tap.Contracts.Features.Reviews;

public record UpdateReviewRequest(int Id, string Title, string Content, int Rating);
