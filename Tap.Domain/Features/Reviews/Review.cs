using Tap.Domain.Core.Abstraction;
using Tap.Domain.Core.Errors;
using Tap.Domain.Core.Primitives;
using Tap.Domain.Core.Primitives.Result;
using Tap.Domain.Core.Utility;
using Tap.Domain.Features.Reviews.Events;
using Hotel = Tap.Domain.Features.Hotels.Hotel;
using User = Tap.Domain.Features.Users.User;

namespace Tap.Domain.Features.Reviews;

public class Review : AggregateRoot, IAuditableEntity
{
    private Review() { }

    private Review(string title, string content, int rating, Hotel hotel, User user)
    {
        Title = title;
        Content = content;
        Rating = rating;
        HotelId = hotel.Id;
        UserId = user.Id;
    }

    public static Review Create(string title, string content, int rating, Hotel hotel, User user)
    {
        Ensure.NotEmpty(title, nameof(title), "Review title is required.");
        Ensure.NotEmpty(content, nameof(content), "Review content is required.");
        Ensure.GreaterThan(rating, 0, nameof(rating), "Review rating must be greater than 0.");
        Ensure.LessThan(rating, 6, nameof(rating), "Review rating must be less than 6.");

        return new Review(title, content, rating, hotel, user);
    }

    public string Title { get; private set; }
    public string Content { get; private set; }
    public int Rating { get; private set; }
    public int HotelId { get; private set; }
    public int UserId { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime? UpdatedAtUtc { get; private set; }

    public Result Update(string title, string content, int rating)
    {
        Ensure.NotEmpty(title, nameof(title), "Review title is required.");
        Ensure.NotEmpty(content, nameof(content), "Review content is required.");
        Ensure.GreaterThan(rating, 0, nameof(rating), "Review rating must be greater than 0.");
        Ensure.LessThan(rating, 6, nameof(rating), "Review rating must be less than 6.");

        if (Title == title && Content == content && Rating == rating)
        {
            return DomainErrors.Review.NothingToUpdate;
        }

        Title = title;
        Content = content;
        Rating = rating;

        AddDomainEvent(new ReviewUpdatedEvent(HotelId));

        return Result.Success();
    }
}
