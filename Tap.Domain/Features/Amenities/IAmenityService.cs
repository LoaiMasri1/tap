using Tap.Domain.Core.Primitives.Result;

namespace Tap.Domain.Features.Amenities;

public interface IAmenityService
{
    Task<Result> CheckAmenityTypeAndUserOwnerShipAsync(
        int userId,
        int typeId,
        AmenityType amenityType,
        CancellationToken cancellationToken = default
    );
}
