using Tap.Application.Core.Messaging;
using Tap.Contracts.Features.Amenities;
using Tap.Domain.Core.Primitives.Result;

namespace Tap.Application.Features.Amenities.UpdateAmenity;

public record UpdateAmenityCommand(int Id, string Name, string Description)
    : ICommand<Result<AmenityResponse>>;
