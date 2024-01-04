using Tap.Application.Core.Messaging;
using Tap.Contracts.Features.Rooms;
using Tap.Domain.Core.Primitives.Maybe;

namespace Tap.Application.Features.Rooms.GetRoomById;

public record GetRoomByIdQuery(int Id) : IQuery<Maybe<FilteredRoomResponse>>;
