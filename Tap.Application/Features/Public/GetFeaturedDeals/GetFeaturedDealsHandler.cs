using Microsoft.EntityFrameworkCore;
using Tap.Application.Core.Abstractions.Data;
using Tap.Application.Core.Messaging;
using Tap.Contracts.Features.Public;
using Tap.Domain.Common.Enumerations;
using Tap.Domain.Core.Primitives.Maybe;
using Tap.Domain.Features.Hotels;
using Tap.Domain.Features.Photos;

namespace Tap.Application.Features.Public.GetFeaturedDeals;

public class GetFeaturedDealsHandler
    : IQueryHandler<GetFeaturedDealsQuery, Maybe<FeaturedDealResponse[]>>
{
    private readonly IDbContext _dbContext;

    public GetFeaturedDealsHandler(IDbContext dbContext) => _dbContext = dbContext;

    public async Task<Maybe<FeaturedDealResponse[]>> Handle(
        GetFeaturedDealsQuery request,
        CancellationToken cancellationToken
    )
    {
        var featuredDeals = await _dbContext
            .Set<Hotel>()
            .AsNoTracking()
            .Include(h => h.City)
            .Include(h => h.Rooms)
            .ThenInclude(r => r.Discounts)
            .Where(h => h.Rooms.Any(r => r.IsAvailable))
            .OrderByDescending(h => h.Rooms.Count(r => r.IsAvailable))
            .Take(request.Limit)
            .ToListAsync(cancellationToken);

        var photos = _dbContext.Set<Photo>().Where(p => p.Type == ItemType.Hotel);

        var result = featuredDeals
            .Select(h =>
            {
                var minPrice = h.Rooms.Min(r => r.Price.Amount);
                var maxDiscount = h.Rooms
                    .SelectMany(r => r.Discounts)
                    .MaxBy(d => d.DiscountPercentage)
                    ?.DiscountPercentage;

                return new FeaturedDealResponse(
                    h.Name,
                    photos.FirstOrDefault(p => p.ItemId == h.Id)?.Url,
                    h.Location.Latitude,
                    h.Location.Longitude,
                    h.City.Name,
                    h.Rating,
                    minPrice,
                    maxDiscount,
                    minPrice - minPrice * maxDiscount / 100
                );
            })
            .ToArray();

        return result;
    }
}
