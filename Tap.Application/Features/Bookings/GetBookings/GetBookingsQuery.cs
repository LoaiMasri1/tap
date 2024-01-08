using Tap.Application.Core.Messaging;
using Tap.Contracts.Features.Bookings;
using Tap.Domain.Common.QueryOptions;
using Tap.Domain.Core.Primitives.Maybe;

namespace Tap.Application.Features.Bookings.GetBookings;

public record GetBookingsQuery(string Filters, string Sorts, int Page, int PageSize)
    : IQuery<Maybe<BookingResponse[]>>,
        IPageable,
        IFilterable,
        ISortable;
