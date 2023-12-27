using Tap.Application.Core.Abstractions.Data;
using Tap.Application.Core.Messaging;
using Tap.Application.Features.Authentication;
using Tap.Domain.Core.Errors;
using Tap.Domain.Core.Primitives.Result;
using Tap.Domain.Features.Rooms;
using Tap.Domain.Features.Users;

namespace Tap.Application.Features.Rooms.DeleteRoom;

public class DeleteRoomCommandHandler : ICommandHandler<DeleteRoomCommand, Result>
{
    private readonly IRoomRepository _roomRepository;
    private readonly IUserIdentifierProvider _userIdentifierProvider;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteRoomCommandHandler(
        IUnitOfWork unitOfWork,
        IRoomRepository roomRepository,
        IUserIdentifierProvider userIdentifierProvider
    )
    {
        _unitOfWork = unitOfWork;
        _roomRepository = roomRepository;
        _userIdentifierProvider = userIdentifierProvider;
    }

    public async Task<Result> Handle(DeleteRoomCommand command, CancellationToken cancellationToken)
    {
        var userRole = _userIdentifierProvider.Role;

        if (userRole != UserRole.Admin)
        {
            return DomainErrors.User.Unauthorized;
        }

        var maybeRoom = await _roomRepository.GetByIdAsync(command.Id, cancellationToken);

        if (maybeRoom.HasNoValue)
        {
            return DomainErrors.Room.NotFound;
        }

        var room = maybeRoom.Value;

        _roomRepository.Remove(room);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
