using Tap.Application.Core.Abstractions.Data;
using Tap.Application.Core.Messaging;
using Tap.Application.Features.Authentication;
using Tap.Contracts.Features.Rooms;
using Tap.Domain.Common.ValueObjects;
using Tap.Domain.Core.Errors;
using Tap.Domain.Core.Primitives.Result;
using Tap.Domain.Features.Hotels;
using Tap.Domain.Features.Rooms;
using Tap.Domain.Features.Users;

namespace Tap.Application.Features.Rooms.CreateRoom;

public class CreateRoomCommandHandler : ICommandHandler<CreateRoomCommand, Result<RoomResponse>>
{
    private readonly IHotelRepository _hotelRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserIdentifierProvider _userIdentifierProvider;

    public CreateRoomCommandHandler(
        IHotelRepository hotelRepository,
        IUnitOfWork unitOfWork,
        IUserIdentifierProvider userIdentifierProvider
    )
    {
        _hotelRepository = hotelRepository;
        _unitOfWork = unitOfWork;
        _userIdentifierProvider = userIdentifierProvider;
    }

    public async Task<Result<RoomResponse>> Handle(
        CreateRoomCommand command,
        CancellationToken cancellationToken
    )
    {
        var userRole = _userIdentifierProvider.Role;
        if (userRole != UserRole.Admin)
        {
            return DomainErrors.User.Unauthorized;
        }

        var maybeHotel = await _hotelRepository.GetByIdAsync(command.HotelId, cancellationToken);

        if (maybeHotel.HasNoValue)
        {
            return DomainErrors.Hotel.NotFound;
        }

        var hotel = maybeHotel.Value;

        var price = Money.Create(command.Price, command.Currency);

        var room = Room.Create(
            command.Number,
            price,
            command.Type,
            command.CapacityOfAdults,
            command.CapacityOfChildren
        );

        hotel.AddRoom(room);

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
