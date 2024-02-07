using Tap.Application.Core.Messaging;
using Tap.Contracts.Features.Amenities;
using Tap.Domain.Core.Primitives.Result;
using Tap.Domain.Features.Amenities;

namespace Tap.Application.Features.Amenities.CreateAmenity;

public record CreateAmenityCommand(string Name, string Description, AmenityType Type, int TypeId)
    : ICommand<Result<AmenityResponse>>;
