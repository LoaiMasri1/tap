using Tap.Application.Core.Abstractions.Data;
using Tap.Application.Core.Messaging;
using Tap.Application.Features.Authentication;
using Tap.Domain.Core.Errors;
using Tap.Domain.Core.Primitives.Result;
using Tap.Domain.Features.Bookings;
using Tap.Domain.Features.Users;

namespace Tap.Application.Features.Bookings.ConfirmBooking;

public record ConfirmBookingCommandHandler : ICommandHandler<ConfirmBookingCommand, Result>
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;

    public ConfirmBookingCommandHandler(
        IBookingRepository bookingRepository,
        IUnitOfWork unitOfWork,
        IUserContext userContext
    )
    {
        _bookingRepository = bookingRepository;
        _unitOfWork = unitOfWork;
        _userContext = userContext;
    }

    public async Task<Result> Handle(
        ConfirmBookingCommand request,
        CancellationToken cancellationToken
    )
    {
        if (_userContext.Role != UserRole.Admin)
        {
            return DomainErrors.User.Unauthorized;
        }

        var maybeBooking = await _bookingRepository.GetByIdIncludingHotelAsync(
            request.BookingId,
            cancellationToken
        );

        if (maybeBooking.HasNoValue)
        {
            return DomainErrors.Booking.NotFound;
        }

        var booking = maybeBooking.Value;

        if (booking.Hotel.UserId != _userContext.Id)
        {
            return DomainErrors.User.Unauthorized;
        }

        var result = booking.Confirm();

        if (result.IsFailure)
        {
            return result.Error;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
