using Tap.Application.Core.Messaging;
using Tap.Contracts.Features.Hotels;
using Tap.Domain.Common.QueryOptions;
using Tap.Domain.Core.Primitives.Maybe;

namespace Tap.Application.Features.Hotels.SearchHotel;

public record SearchHotelsQuery(
    int? NumberOfAvailableRooms,
    int PageSize,
    int Page,
    string Sorts,
    string Filters
) : IQuery<Maybe<SearchHotelResponse[]>>, IPageable, ISortable, IFilterable;
