using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;
using Tap.Application.Core.Abstractions.Data;
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
    private readonly ISieveProcessor _sieveProcessor;

    public GetBookingsQueryHandler(
        IDbContext dbContext,
        IUserContext userContext,
        ISieveProcessor sieveProcessor
    )
    {
        _dbContext = dbContext;
        _userContext = userContext;
        _sieveProcessor = sieveProcessor;
    }

    public async Task<Maybe<BookingResponse[]>> Handle(
        GetBookingsQuery request,
        CancellationToken cancellationToken
    )
    {
        var userId = _userContext.Id;
        var role = _userContext.Role;

        IQueryable<Booking> bookings = _dbContext.Set<Booking>().Include(b => b.Hotel);

        bookings = role switch
        {
            UserRole.User => bookings.Where(b => b.UserId == userId),
            UserRole.Admin => bookings.Where(b => b.Hotel.UserId == userId),
            _ => bookings
        };

        var sieveModel = new SieveModel
        {
            Filters = request.Filters,
            Sorts = request.Sorts,
            Page = request.Page,
            PageSize = request.PageSize
        };

        return await _sieveProcessor
            .Apply(sieveModel, bookings)
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
