using Tap.Domain.Common.Enumerations;

namespace Tap.Contracts.Features.Photos;

public record GetPhotoResponse(string Url, int ItemId, ItemType ItemType);
