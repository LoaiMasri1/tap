using Tap.Domain.Core.Errors;
using Tap.Domain.Core.Primitives.Result;
using Tap.Domain.Features.Hotels;

namespace Tap.Domain.Features.Amenities;

public class AmenityService : IAmenityService
{
    private readonly IHotelRepository _hotelRepository;

    public AmenityService(IHotelRepository hotelRepository)
    {
        _hotelRepository = hotelRepository;
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
            AmenityType.Room
                => CheckRoomAmenityTypeAndUserOwnerShipAsync(userId, typeId, cancellationToken),
            _ => throw new ArgumentOutOfRangeException(nameof(amenityType), amenityType, null)
        };

    private Task<Result> CheckRoomAmenityTypeAndUserOwnerShipAsync(
        int userId,
        int typeId,
        CancellationToken cancellationToken = default
    )
    {
        // TODO: Implement this method when the room feature is implemented.
        throw new NotImplementedException();
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
