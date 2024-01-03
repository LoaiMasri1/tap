using Tap.Application.Core.Abstractions.Data;
using Tap.Application.Core.Messaging;
using Tap.Domain.Core.Errors;
using Tap.Domain.Core.Primitives.Result;
using Tap.Domain.Features.Bookings;

namespace Tap.Application.Features.Payments.SuccessPayment;

public class SuccessPaymentCommandHandler : ICommandHandler<SuccessPaymentCommand, Result>
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SuccessPaymentCommandHandler(
        IBookingRepository bookingRepository,
        IUnitOfWork unitOfWork
    )
    {
        _bookingRepository = bookingRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(
        SuccessPaymentCommand request,
        CancellationToken cancellationToken
    )
    {
        var maybeBooking = await _bookingRepository.GetBySessionIdAsync(
            request.SessionId,
            cancellationToken
        );

        if (maybeBooking.HasNoValue)
        {
            return DomainErrors.Booking.NotFound;
        }

        var booking = maybeBooking.Value;

        var result = booking.Pay();

        if (result.IsFailure)
        {
            return result.Error;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
