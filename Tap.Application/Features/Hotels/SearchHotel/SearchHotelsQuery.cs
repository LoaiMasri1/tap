using Tap.Application.Core.Messaging;
using Tap.Contracts.Features.Hotels;
using Tap.Domain.Core.Primitives.Maybe;

namespace Tap.Application.Features.Hotels.SearchHotel;

public record SearchHotelsQuery(
    string? City,
    int? NumberOfAvailableRooms,
    int PageSize,
    int PageNumber,
    string SortBy,
    string SortOrder,
    string? FilterBy,
    string? FilterQuery
) : IQuery<Maybe<SearchHotelResponse[]>>, IPageable, ISortable, IFilterable;
