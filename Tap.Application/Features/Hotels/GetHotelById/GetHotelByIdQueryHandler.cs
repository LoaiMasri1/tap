using Microsoft.EntityFrameworkCore;
using Tap.Application.Core.Abstractions.Data;
using Tap.Application.Core.Messaging;
using Tap.Contracts.Features.Amenities;
using Tap.Contracts.Features.Hotels;
using Tap.Contracts.Features.Rooms;
using Tap.Domain.Common.Enumerations;
using Tap.Domain.Core.Primitives.Maybe;
using Tap.Domain.Features.Amenities;
using Tap.Domain.Features.Hotels;
using Tap.Domain.Features.Photos;
using Tap.Domain.Features.Rooms;

namespace Tap.Application.Features.Hotels.GetHotelById;

public class GetHotelByIdQueryHandler : IQueryHandler<GetHotelByIdQuery, Maybe<SearchHotelResponse>>
{
    private readonly IDbContext _dbContext;

    public GetHotelByIdQueryHandler(IDbContext dbContext) => _dbContext = dbContext;

    public async Task<Maybe<SearchHotelResponse>> Handle(
        GetHotelByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        var hotel = await _dbContext
            .Set<Hotel>()
            .AsNoTracking()
            .Include(h => h.City)
            .Include(h => h.Rooms)
            .ThenInclude(r => r.Discounts)
            .FirstOrDefaultAsync(h => h.Id == request.Id, cancellationToken);

        if (hotel is null)
        {
            return Maybe<SearchHotelResponse>.None;
        }

        var amenities = GetAmenities(hotel.Id, AmenityType.Hotel);
        var photos = GetPhotos(hotel.Id, ItemType.Hotel);
        var rooms = request.IncludeRooms ? GetRooms(hotel) : null;

        var hotelResponse = CreateHotelResponse(hotel, amenities, photos, rooms);
        return hotelResponse;
    }

    private IEnumerable<FilteredAmenityResponse> GetAmenities(int id, AmenityType type) =>
        GetAmenityByType(id, type).Select(a => new FilteredAmenityResponse(a.Name, a.Description));

    private IEnumerable<PhotoResponse> GetPhotos(int id, ItemType type) =>
        GetPhotosByType(id, type).Select(p => new PhotoResponse(p.Url));

    private IEnumerable<FilteredRoomResponse> GetRooms(Hotel hotel) =>
        hotel.Rooms.Select(CreateRoomResponse);

    private static SearchHotelResponse CreateHotelResponse(
        Hotel hotel,
        IEnumerable<FilteredAmenityResponse> amenities,
        IEnumerable<PhotoResponse> photos,
        IEnumerable<FilteredRoomResponse>? rooms
    ) =>
        new(
            hotel.Id,
            hotel.Name,
            hotel.City.Name,
            hotel.Rating,
            hotel.Location.Longitude,
            hotel.Location.Latitude,
            hotel.Rooms.Count(r => r.IsAvailable),
            amenities.ToArray(),
            photos.ToArray(),
            rooms?.ToArray()
        );

    private FilteredRoomResponse CreateRoomResponse(Room room) =>
        new(
            room.Id,
            room.Number,
            room.Type,
            room.CapacityOfAdults,
            room.CapacityOfChildren,
            room.Discounts.MaxBy(d => d.DiscountPercentage)?.DiscountPercentage ?? 0,
            room.Price.Amount,
            room.DiscountedPrice,
            room.Price.Currency,
            room.IsAvailable,
            GetAmenities(room.Id, AmenityType.Room).ToArray(),
            GetPhotos(room.Id, ItemType.Room).ToArray()
        );

    private IQueryable<Photo> GetPhotosByType(int id, ItemType type) =>
        _dbContext.Set<Photo>().AsNoTracking().Where(x => x.Type == type && x.ItemId == id);

    private IQueryable<Amenity> GetAmenityByType(int id, AmenityType type) =>
        _dbContext.Set<Amenity>().AsNoTracking().Where(x => x.Type == type && x.TypeId == id);
}
