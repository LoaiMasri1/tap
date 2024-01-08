using Tap.Application.Core.Messaging;
using Tap.Contracts.Features.Discounts;
using Tap.Domain.Common.QueryOptions;
using Tap.Domain.Core.Primitives.Maybe;

namespace Tap.Application.Features.Discounts.GetDiscountsForSpecificHotel;

public record GetDiscountsForSpecificHotelQuery(
    int Page,
    int PageSize,
    string Sorts,
    string Filters,
    int HotelId
) : IQuery<Maybe<FilteredDiscountResponse[]>>, IPageable, ISortable, IFilterable;
