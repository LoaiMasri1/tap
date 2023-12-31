using Tap.Application.Core.Messaging;
using Tap.Contracts.Features.Hotels;
using Tap.Domain.Core.Primitives.Maybe;

namespace Tap.Application.Features.Hotels.SearchHotel;

public record SearchHotelsQuery(
    string City,
    int? Rating,
    int? NumberOfAvailableRooms,
    int PageSize,
    int PageNumber,
    string SortBy,
    string SortOrder
) : IPagedQuery<Maybe<SearchHotelResponse[]>>;
