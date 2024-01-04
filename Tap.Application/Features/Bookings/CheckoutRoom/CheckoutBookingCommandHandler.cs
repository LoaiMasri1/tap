using Tap.Application.Core.Abstractions.Data;
using Tap.Application.Core.Abstractions.Sessions;
using Tap.Application.Core.Messaging;
using Tap.Application.Features.Authentication;
using Tap.Domain.Common;
using Tap.Domain.Core.Errors;
using Tap.Domain.Core.Primitives.Result;
using Tap.Domain.Features.Bookings;
using Tap.Domain.Features.Users;

namespace Tap.Application.Features.Bookings.CheckoutRoom;

public class CheckoutBookingCommandHandler
    : ICommandHandler<CheckoutBookingCommand, Result<Session>>
{
    private readonly IBookingRepository _bookingRepository;
    private readonly ISessionService _sessionService;
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _unitOfWork;

    public CheckoutBookingCommandHandler(
        IBookingRepository bookingRepository,
        ISessionService sessionService,
        IUserContext userContext,
        IUnitOfWork unitOfWork
    )
    {
        _bookingRepository = bookingRepository;
        _sessionService = sessionService;
        _userContext = userContext;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Session>> Handle(
        CheckoutBookingCommand command,
        CancellationToken cancellation
    )
    {
        if (_userContext.Role != UserRole.User)
        {
            return DomainErrors.User.Unauthorized;
        }

        var maybeBooking = await _bookingRepository.GetByIdAsync(command.BookingId, cancellation);

        if (maybeBooking.HasNoValue)
        {
            return DomainErrors.Booking.NotFound;
        }

        var booking = maybeBooking.Value;

        if (booking.UserId != _userContext.Id)
        {
            return DomainErrors.User.Unauthorized;
        }

        var maybeSession = await _sessionService.SaveAsync(command.BookingId, cancellation);

        if (maybeSession.HasNoValue)
        {
            return DomainErrors.Session.NotFound;
        }

        booking.AddSession(maybeSession.Value.SessionId);

        await _unitOfWork.SaveChangesAsync(cancellation);

        return maybeSession.Value;
    }
}
