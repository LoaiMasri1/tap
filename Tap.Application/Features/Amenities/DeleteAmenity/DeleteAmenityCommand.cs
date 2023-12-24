using Tap.Application.Core.Messaging;
using Tap.Domain.Core.Primitives.Result;

namespace Tap.Application.Features.Amenities.DeleteAmenity;

public record DeleteAmenityCommand(int Id) : ICommand<Result>;
