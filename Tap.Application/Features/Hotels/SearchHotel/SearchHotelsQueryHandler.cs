using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;
using Tap.Application.Core.Abstractions.Data;
using Tap.Application.Core.Messaging;
using Tap.Contracts.Features.Amenities;
using Tap.Contracts.Features.Hotels;
using Tap.Contracts.Features.Photos;
using Tap.Domain.Common.Enumerations;
using Tap.Domain.Core.Primitives.Maybe;
using Tap.Domain.Features.Amenities;
using Tap.Domain.Features.Hotels;
using Tap.Domain.Features.Photos;

namespace Tap.Application.Features.Hotels.SearchHotel;

public class SearchHotelsQueryHandler
    : IQueryHandler<SearchHotelsQuery, Maybe<SearchHotelResponse[]>>
{
    private readonly IDbContext _dbContext;
    private readonly ISieveProcessor _sieveProcessor;

    public SearchHotelsQueryHandler(IDbContext dbContext, ISieveProcessor sieveProcessor)
    {
        _dbContext = dbContext;
        _sieveProcessor = sieveProcessor;
    }

    public async Task<Maybe<SearchHotelResponse[]>> Handle(
        SearchHotelsQuery request,
        CancellationToken cancellationToken
    )
    {
        var amenities = _dbContext.Set<Amenity>().Where(a => a.Type == AmenityType.Hotel);
        var photos = _dbContext.Set<Photo>().Where(p => p.Type == ItemType.Room);

        var hotels = _dbContext
            .Set<Hotel>()
            .AsNoTracking()
            .Include(h => h.City)
            .Include(h => h.Rooms);

        var sieveModel = new SieveModel
        {
            Filters = request.Filters,
            Sorts = request.Sorts,
            Page = request.Page,
            PageSize = request.PageSize
        };

        return await _sieveProcessor
            .Apply(sieveModel, hotels)
            .Select(
                h =>
                    new SearchHotelResponse(
                        h.Id,
                        h.Name,
                        h.Description,
                        h.City.Name,
                        h.Rating,
                        h.Location.Longitude,
                        h.Location.Latitude,
                        h.Rooms.Count(r => r.IsAvailable),
                        amenities
                            .Where(a => a.TypeId == h.Id)
                            .Select(a => new FilteredAmenityResponse(a.Name, a.Description))
                            .ToArray(),
                        photos
                            .Where(p => p.ItemId == h.Id)
                            .Select(p => new PhotoResponse(p.Url))
                            .ToArray(),
                        null
                    )
            )
            .ToArrayAsync(cancellationToken);
    }
}
