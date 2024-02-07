using Tap.Application.Core.Messaging;
using Tap.Domain.Core.Primitives.Result;

namespace Tap.Application.Features.Rooms.DeleteRoom;

public record DeleteRoomCommand(int Id) : ICommand<Result>;
