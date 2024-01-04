using Microsoft.EntityFrameworkCore;
using Tap.Application.Core.Abstractions.Data;
using Tap.Application.Core.Messaging;
using Tap.Application.Features.Hotels;
using Tap.Contracts.Features.Amenities;
using Tap.Contracts.Features.Rooms;
using Tap.Domain.Common.Enumerations;
using Tap.Domain.Core.Primitives.Maybe;
using Tap.Domain.Features.Amenities;
using Tap.Domain.Features.Photos;
using Tap.Domain.Features.Rooms;

namespace Tap.Application.Features.Rooms.GetRoomById;

public class GetRoomByIdQueryHandler : IQueryHandler<GetRoomByIdQuery, Maybe<FilteredRoomResponse>>
{
    private readonly IDbContext _dbContext;

    public GetRoomByIdQueryHandler(IDbContext dbContext) => _dbContext = dbContext;

    public async Task<Maybe<FilteredRoomResponse>> Handle(
        GetRoomByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        var room = await _dbContext
            .Set<Room>()
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken);

        if (room is null)
        {
            return Maybe<FilteredRoomResponse>.None;
        }

        var amenities = _dbContext.Set<Amenity>().Where(a => a.Type == AmenityType.Room);

        var photos = _dbContext.Set<Photo>().Where(p => p.Type == ItemType.Room);

        var roomResponse = new FilteredRoomResponse(
            room.Id,
            room.Number,
            room.Type,
            room.CapacityOfAdults,
            room.CapacityOfChildren,
            room.GetDiscountedPercentage(),
            room.Price.Amount,
            room.DiscountedPrice,
            room.Price.Currency,
            room.IsAvailable,
            amenities
                .Where(a => a.TypeId == room.Id)
                .Select(x => new FilteredAmenityResponse(x.Name, x.Description))
                .ToArray(),
            photos.Where(p => p.ItemId == room.Id).Select(x => new PhotoResponse(x.Url)).ToArray()
        );

        return roomResponse;
    }
}
