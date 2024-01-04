using Microsoft.EntityFrameworkCore;
using Tap.Application.Core.Abstractions.Data;
using Tap.Application.Core.Messaging;
using Tap.Application.Features.Authentication;
using Tap.Domain.Common.Enumerations;
using Tap.Domain.Core.Primitives.Maybe;
using Tap.Domain.Features.Bookings;
using Tap.Domain.Features.Photos;

namespace Tap.Application.Features.Users.GetRecentVisits;

public record RecentVisitsResponse(
    string HotelName,
    string? ImageUrl,
    string CityName,
    int Rating,
    decimal Price,
    string CheckInDate,
    string CheckOutDate
);

public record GetRecentVisitsQuery(int Limit) : IQuery<Maybe<RecentVisitsResponse[]>>;

public class GetRecentVisitsQueryHandler
    : IQueryHandler<GetRecentVisitsQuery, Maybe<RecentVisitsResponse[]>>
{
    private readonly IDbContext _dbContext;
    private readonly IUserContext _userContext;

    public GetRecentVisitsQueryHandler(IDbContext dbContext, IUserContext userContext)
    {
        _dbContext = dbContext;
        _userContext = userContext;
    }

    public async Task<Maybe<RecentVisitsResponse[]>> Handle(
        GetRecentVisitsQuery request,
        CancellationToken cancellationToken
    )
    {
        var userId = _userContext.Id;

        var photos = _dbContext.Set<Photo>().Where(p => p.Type == ItemType.Hotel);

        var recentVisits = await _dbContext
            .Set<Booking>()
            .Include(r => r.Hotel)
            .ThenInclude(h => h.City)
            .Where(v => v.UserId == userId)
            .OrderByDescending(v => v.CheckInDate)
            .Take(request.Limit)
            .ToListAsync(cancellationToken);

        var result = recentVisits
            .Select(
                v =>
                    new RecentVisitsResponse(
                        v.Hotel.Name,
                        photos.FirstOrDefault(p => p.ItemId == v.Hotel.Id)?.Url,
                        v.Hotel.City.Name,
                        v.Hotel.Rating,
                        v.TotalPrice,
                        v.CheckInDate.ToString("dd/MM/yyyy"),
                        v.CheckOutDate.ToString("dd/MM/yyyy")
                    )
            )
            .ToArray();

        return result;
    }
}
