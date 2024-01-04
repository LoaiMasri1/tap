using Microsoft.EntityFrameworkCore;
using Tap.Application.Core.Abstractions.Data;
using Tap.Application.Core.Messaging;
using Tap.Contracts.Features.Public;
using Tap.Domain.Common.Enumerations;
using Tap.Domain.Core.Primitives.Maybe;
using Tap.Domain.Features.Cities;
using Tap.Domain.Features.Photos;

namespace Tap.Application.Features.Public.GetTrendingDestination
{
    public class GetTrendingDestinationHandler
        : IQueryHandler<GetTrendingDestinationQuery, Maybe<TrendingDestinationResponse[]>>
    {
        private readonly IDbContext _dbContext;

        public GetTrendingDestinationHandler(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Maybe<TrendingDestinationResponse[]>> Handle(
            GetTrendingDestinationQuery request,
            CancellationToken cancellationToken
        )
        {
            var photos = _dbContext.Set<Photo>().Where(p => p.Type == ItemType.City);

            var trendingDestinations = await _dbContext
                .Set<City>()
                .AsNoTracking()
                .Include(c => c.Hotels)
                .ThenInclude(h => h.Rooms)
                .ThenInclude(r => r.Bookings)
                .ToListAsync(cancellationToken);

            var orderedDestinations = trendingDestinations
                .Select(c => new { City = c, TrendingScore = c.CalculateTrendingScore() })
                .OrderByDescending(x => x.TrendingScore)
                .Take(request.Limit);

            var result = orderedDestinations
                .Select(
                    x =>
                        new TrendingDestinationResponse(
                            x.City.Name,
                            photos.FirstOrDefault(p => p.ItemId == x.City.Id)?.Url,
                            x.City.Description,
                            x.City.Name,
                            x.City.Country
                        )
                )
                .ToArray();

            return result;
        }
    }
}
