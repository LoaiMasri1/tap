using Tap.Application.Core.Messaging;
using Tap.Contracts.Features.Photos;
using Tap.Domain.Common.QueryOptions;
using Tap.Domain.Core.Primitives.Maybe;

namespace Tap.Application.Features.Photos.GetPhotos;

public record GetPhotosQuery(int Page, int PageSize, string Filters)
    : IQuery<Maybe<GetPhotoResponse[]>>,
        IPageable,
        IFilterable;
