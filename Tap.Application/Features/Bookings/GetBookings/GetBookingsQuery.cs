using Tap.Application.Core.Messaging;
using Tap.Contracts.Features.Bookings;
using Tap.Domain.Common.QueryOptions;
using Tap.Domain.Core.Primitives.Maybe;

namespace Tap.Application.Features.Bookings.GetBookings;

public record GetBookingsQuery(
    string? FilterBy,
    string? FilterQuery,
    string SortBy,
    string SortOrder,
    int PageNumber,
    int PageSize
) : IQuery<Maybe<BookingResponse[]>>, IPageable, IFilterable, ISortable;
