using Microsoft.EntityFrameworkCore;
using Tap.Application.Core.Abstractions.Data;
using Tap.Application.Core.Extensions;
using Tap.Application.Core.Messaging;
using Tap.Contracts.Features.Amenities;
using Tap.Contracts.Features.Hotels;
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

    public SearchHotelsQueryHandler(IDbContext dbContext) => _dbContext = dbContext;

    public async Task<Maybe<SearchHotelResponse[]>> Handle(
        SearchHotelsQuery request,
        CancellationToken cancellationToken
    )
    {
        var amenities = _dbContext.Set<Amenity>().Where(a => a.Type == AmenityType.Hotel);
        var photos = _dbContext.Set<Photo>().Where(p => p.Type == ItemType.Room);

        var hotels = await _dbContext
            .Set<Hotel>()
            .AsNoTracking()
            .Include(h => h.City)
            .Include(h => h.Rooms)
            .Where(h => request.City == null || h.City.Name == request.City)
            .Where(
                h =>
                    request.NumberOfAvailableRooms == null
                    || h.Rooms.Count(r => r.IsAvailable) == request.NumberOfAvailableRooms
            )
            .OrderBy(request.SortBy, request.SortOrder)
            .FilterBy(request.FilterBy, request.FilterQuery)
            .Paginate(request.PageNumber, request.PageSize)
            .Select(
                h =>
                    new SearchHotelResponse(
                        h.Id,
                        h.Name,
                        h.City.Name,
                        h.Description,
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

        return hotels;
    }
}
