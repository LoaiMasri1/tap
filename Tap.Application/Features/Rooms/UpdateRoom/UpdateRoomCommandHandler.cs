using Tap.Application.Core.Abstractions.Data;
using Tap.Application.Core.Messaging;
using Tap.Application.Features.Authentication;
using Tap.Contracts.Features.Rooms;
using Tap.Domain.Common.ValueObjects;
using Tap.Domain.Core.Errors;
using Tap.Domain.Core.Primitives.Result;
using Tap.Domain.Features.Rooms;
using Tap.Domain.Features.Users;

namespace Tap.Application.Features.Rooms.UpdateRoom;

public class UpdateRoomCommandHandler : ICommandHandler<UpdateRoomCommand, Result<RoomResponse>>
{
    private readonly IRoomRepository _roomRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserIdentifierProvider _userIdentifierProvider;

    public UpdateRoomCommandHandler(
        IUnitOfWork unitOfWork,
        IUserIdentifierProvider userIdentifierProvider,
        IRoomRepository roomRepository
    )
    {
        _unitOfWork = unitOfWork;
        _userIdentifierProvider = userIdentifierProvider;
        _roomRepository = roomRepository;
    }

    public async Task<Result<RoomResponse>> Handle(
        UpdateRoomCommand command,
        CancellationToken cancellationToken
    )
    {
        var userRole = _userIdentifierProvider.Role;
        if (userRole != UserRole.Admin)
        {
            return DomainErrors.User.Unauthorized;
        }

        var maybeRoom = await _roomRepository.GetByIdWithDiscountsAsync(
            command.Id,
            cancellationToken
        );

        if (maybeRoom.HasNoValue)
        {
            return DomainErrors.Room.NotFound;
        }

        var room = maybeRoom.Value;

        var price = Money.Create(command.Price, command.Currency);

        room.Update(
            command.Number,
            price,
            command.Type,
            command.CapacityOfAdults,
            command.CapacityOfChildren
        );

        room.UpdateDiscountedPrice();

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new RoomResponse(
            room.Id,
            room.Number,
            room.Price,
            room.DiscountedPrice,
            room.Type,
            room.IsAvailable,
            room.CapacityOfAdults,
            room.CapacityOfChildren
        );
    }
}
