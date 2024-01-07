using Tap.Application.Core.Messaging;
using Tap.Contracts.Features.Amenities;
using Tap.Domain.Common.QueryOptions;
using Tap.Domain.Core.Primitives.Maybe;

namespace Tap.Application.Features.Amenities.GetAmenities;

public record GetAmenitiesQuery(
    string? FilterBy,
    string? FilterQuery,
    string SortBy,
    string SortOrder,
    int PageNumber,
    int PageSize
) : IQuery<Maybe<AmenityResponse[]>>, IPageable, ISortable, IFilterable;
