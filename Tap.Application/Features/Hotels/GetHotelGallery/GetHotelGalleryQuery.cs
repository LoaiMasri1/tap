using Tap.Application.Core.Messaging;
using Tap.Contracts.Features.Photos;
using Tap.Domain.Common.QueryOptions;
using Tap.Domain.Core.Primitives.Maybe;

namespace Tap.Application.Features.Hotels.GetHotelGallery;

public record GetHotelGalleryQuery(int Id, int PageNumber, int PageSize)
    : IQuery<Maybe<PhotoResponse[]>>,
        IPageable;
