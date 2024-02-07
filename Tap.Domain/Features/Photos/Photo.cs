using Tap.Domain.Common.Enumerations;
using Tap.Domain.Core.Abstraction;
using Tap.Domain.Core.Errors;
using Tap.Domain.Core.Primitives;
using Tap.Domain.Core.Primitives.Result;

namespace Tap.Domain.Features.Photos;

public class Photo : Entity, IAuditableEntity
{
    private Photo() { }

    private Photo(string url, ItemType type, int itemId)
    {
        Url = url;
        Type = type;
        ItemId = itemId;
    }

    public static Result<Photo> Create(string url, ItemType type, int itemId) =>
        Result
            .Create((url, type, itemId))
            .Ensure(x => !string.IsNullOrWhiteSpace(x.url), DomainErrors.Photo.UrlNullOrEmpty)
            //.Ensure(
            //    x => Uri.TryCreate(url, UriKind.Absolute, out _),
            //    DomainErrors.Photo.UrlInvalidFormat
            //)
            .Ensure(x => Enum.IsDefined(typeof(ItemType), x.type), DomainErrors.Photo.TypeInvalid)
            .Ensure(x => x.itemId > 0, DomainErrors.Photo.ItemIdInvalid)
            .Map(x => new Photo(x.url, x.type, x.itemId));

    public string Url { get; private set; }
    public ItemType Type { get; private set; }
    public int ItemId { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime? UpdatedAtUtc { get; private set; }

    public void UpdateUrl(string url)
    {
        Url = url;
    }
}
