using Microsoft.EntityFrameworkCore;
using Tap.Application.Core.Abstractions.Data;
using Tap.Application.Core.Extensions;
using Tap.Application.Core.Messaging;
using Tap.Application.Features.Authentication;
using Tap.Contracts.Features.Bookings;
using Tap.Domain.Core.Primitives.Maybe;
using Tap.Domain.Features.Bookings;
using Tap.Domain.Features.Users;

namespace Tap.Application.Features.Bookings.GetBookings;

public class GetBookingsQueryHandler : IQueryHandler<GetBookingsQuery, Maybe<BookingResponse[]>>
{
    private readonly IDbContext _dbContext;
    private readonly IUserContext _userContext;

    public GetBookingsQueryHandler(IDbContext dbContext, IUserContext userContext)
    {
        _dbContext = dbContext;
        _userContext = userContext;
    }

    public async Task<Maybe<BookingResponse[]>> Handle(
        GetBookingsQuery request,
        CancellationToken cancellationToken
    )
    {
        var userId = _userContext.Id;
        var role = _userContext.Role;

        var bookings = _dbContext.Set<Booking>().Include(b => b.Hotel).AsQueryable();

        bookings = role switch
        {
            UserRole.User => bookings.Where(b => b.UserId == userId),
            UserRole.Admin => bookings.Where(b => b.Hotel.UserId == userId),
            _ => bookings
        };

        return await bookings
            .FilterBy(request.FilterBy, request.FilterQuery)
            .OrderBy(request.SortBy, request.SortOrder)
            .Paginate(request.PageNumber, request.PageSize)
            .Select(
                b =>
                    new BookingResponse(
                        b.Id,
                        b.HotelId,
                        b.RoomId,
                        b.UserId,
                        b.Status,
                        b.CheckInDate,
                        b.CheckOutDate,
                        b.TotalPrice
                    )
            )
            .ToArrayAsync(cancellationToken);
    }
}
