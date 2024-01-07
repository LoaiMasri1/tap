using Tap.Application.Core.Messaging;
using Tap.Contracts.Features.Discounts;
using Tap.Domain.Core.Primitives.Maybe;

namespace Tap.Application.Features.Discounts.GetDiscountsForSpecificHotel;

public record GetDiscountsForSpecificHotelQuery(
    int PageNumber,
    int PageSize,
    string SortBy,
    string SortOrder,
    int HotelId
) : IQuery<Maybe<FilteredDiscountResponse[]>>, IPageable, ISortable;
