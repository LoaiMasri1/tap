using MediatR;
using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;
using Tap.Application.Core.Abstractions.Data;
using Tap.Contracts.Features.Amenities;
using Tap.Contracts.Features.Photos;
using Tap.Contracts.Features.Rooms;
using Tap.Domain.Common.Enumerations;
using Tap.Domain.Core.Primitives.Maybe;
using Tap.Domain.Features.Amenities;
using Tap.Domain.Features.Photos;
using Tap.Domain.Features.Rooms;

namespace Tap.Application.Features.Rooms.GetRooms;

public class GetRoomsQueryHandler : IRequestHandler<GetRoomsQuery, Maybe<FilteredRoomResponse[]>>
{
    private readonly IDbContext _dbContext;
    private readonly ISieveProcessor _sieveProcessor;

    public GetRoomsQueryHandler(IDbContext dbContext, ISieveProcessor sieveProcessor)
    {
        _dbContext = dbContext;
        _sieveProcessor = sieveProcessor;
    }

    public async Task<Maybe<FilteredRoomResponse[]>> Handle(
        GetRoomsQuery request,
        CancellationToken cancellationToken
    )
    {
        IQueryable<Room> rooms = _dbContext.Set<Room>().AsNoTracking().Include(r => r.Discounts);

        var sieveModel = new SieveModel
        {
            Filters = request.Filters,
            Sorts = request.Sorts,
            Page = request.Page,
            PageSize = request.PageSize
        };

        rooms = _sieveProcessor.Apply(sieveModel, rooms);

        var amenities = _dbContext.Set<Amenity>().Where(a => a.Type == AmenityType.Room);

        var photos = _dbContext.Set<Photo>().Where(p => p.Type == ItemType.Room);

        var roomsResponse = await rooms
            .Select(
                r =>
                    new FilteredRoomResponse(
                        r.Id,
                        r.Number,
                        r.Type,
                        r.CapacityOfAdults,
                        r.CapacityOfChildren,
                        r.GetDiscountedPercentage(),
                        r.Price.Amount,
                        r.DiscountedPrice,
                        r.Price.Currency,
                        r.IsAvailable,
                        amenities
                            .Where(a => a.TypeId == r.Id)
                            .Select(x => new FilteredAmenityResponse(x.Name, x.Description))
                            .ToArray(),
                        photos
                            .Where(p => p.ItemId == r.Id)
                            .Select(x => new PhotoResponse(x.Url))
                            .ToArray()
                    )
            )
            .ToArrayAsync(cancellationToken);

        return roomsResponse;
    }
}
