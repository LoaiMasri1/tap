using Tap.Application.Core.Messaging;
using Tap.Contracts.Features.Amenities;
using Tap.Domain.Common.QueryOptions;
using Tap.Domain.Core.Primitives.Maybe;

namespace Tap.Application.Features.Amenities.GetAmenities;

public record GetAmenitiesQuery(string Filters, string Sorts, int Page, int PageSize)
    : IQuery<Maybe<AmenityResponse[]>>,
        IPageable,
        ISortable,
        IFilterable;
