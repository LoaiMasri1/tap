using Tap.Application.Core.Abstractions.Data;
using Tap.Application.Core.Messaging;
using Tap.Application.Features.Authentication;
using Tap.Contracts.Features.Bookings;
using Tap.Domain.Core.Errors;
using Tap.Domain.Core.Primitives.Result;
using Tap.Domain.Features.Bookings;
using Tap.Domain.Features.Rooms;
using Tap.Domain.Features.Users;

namespace Tap.Application.Features.Bookings.BookRoom;

public class BookRoomCommandHandler : ICommandHandler<BookRoomCommand, Result<BookingResponse>>
{
    private readonly IUserRepository _userRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _unitOfWork;

    public BookRoomCommandHandler(
        IUserRepository userRepository,
        IUserContext userContext,
        IUnitOfWork unitOfWork,
        IRoomRepository roomRepository
    )
    {
        _userRepository = userRepository;
        _userContext = userContext;
        _unitOfWork = unitOfWork;
        _roomRepository = roomRepository;
    }

    public async Task<Result<BookingResponse>> Handle(
        BookRoomCommand request,
        CancellationToken cancellationToken
    )
    {
        var userRole = _userContext.Role;

        if (userRole != UserRole.User)
        {
            return DomainErrors.User.Unauthorized;
        }

        var userId = _userContext.Id;

        var maybeUser = await _userRepository.GetByIdAsync(userId, cancellationToken);

        if (maybeUser.HasNoValue)
        {
            return DomainErrors.User.NotFound;
        }

        var maybeRoom = await _roomRepository.GetByIdWithHotelAsync(
            request.RoomId,
            cancellationToken
        );

        if (maybeRoom.HasNoValue)
        {
            return DomainErrors.Room.NotFound;
        }

        var room = maybeRoom.Value;
        var hotel = room.Hotel;

        if (!room.IsAvailable)
        {
            return DomainErrors.Room.NotAvailable;
        }

        var result = room.UpdateAvailability(!room.IsAvailable);

        if (result.IsFailure)
        {
            return result.Error;
        }

        var booking = Booking.Create(
            maybeUser.Value,
            hotel,
            room,
            request.CheckInDate,
            request.CheckOutDate
        );

        hotel.AddBooking(booking);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new BookingResponse(
            booking.Id,
            booking.HotelId,
            booking.RoomId,
            booking.UserId,
            booking.CheckInDate,
            booking.CheckOutDate,
            booking.TotalPrice
        );
    }
}
