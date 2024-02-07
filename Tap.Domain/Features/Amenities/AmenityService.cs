using Tap.Domain.Core.Errors;
using Tap.Domain.Core.Primitives.Result;
using Tap.Domain.Features.Hotels;
using Tap.Domain.Features.Rooms;

namespace Tap.Domain.Features.Amenities;

public class AmenityService : IAmenityService
{
    private readonly IHotelRepository _hotelRepository;
    private readonly IRoomRepository _roomRepository;

    public AmenityService(IHotelRepository hotelRepository, IRoomRepository roomRepository)
    {
        _hotelRepository = hotelRepository;
        _roomRepository = roomRepository;
    }

    public Task<Result> CheckAmenityTypeAndUserOwnerShipAsync(
        int userId,
        int typeId,
        AmenityType amenityType,
        CancellationToken cancellationToken = default
    ) =>
        amenityType switch
        {
            AmenityType.Hotel
                => CheckHotelAmenityTypeAndUserOwnerShipAsync(userId, typeId, cancellationToken),
            AmenityType.Room => CheckRoomAmenityType(typeId, cancellationToken),
            _ => throw new ArgumentOutOfRangeException(nameof(amenityType), amenityType, null)
        };

    private async Task<Result> CheckRoomAmenityType(
        int typeId,
        CancellationToken cancellationToken = default
    )
    {
        var maybeRoom = await _roomRepository.GetByIdAsync(typeId, cancellationToken);
        return maybeRoom.HasNoValue ? DomainErrors.Room.NotFound : Result.Success();
    }

    private async Task<Result> CheckHotelAmenityTypeAndUserOwnerShipAsync(
        int userId,
        int typeId,
        CancellationToken cancellationToken = default
    )
    {
        var maybeHotel = await _hotelRepository.GetByIdAsync(typeId, cancellationToken);

        if (maybeHotel.HasNoValue)
        {
            return DomainErrors.Hotel.NotFound;
        }

        return !IsHotelOwner(userId, maybeHotel.Value)
            ? DomainErrors.User.Unauthorized
            : Result.Success();
    }

    private static bool IsHotelOwner(int userId, Hotel hotel) => hotel.UserId == userId;
}
