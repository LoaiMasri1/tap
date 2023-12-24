using Tap.Application.Core.Abstractions.Data;
using Tap.Application.Core.Messaging;
using Tap.Application.Features.Authentication;
using Tap.Domain.Core.Errors;
using Tap.Domain.Core.Primitives.Result;
using Tap.Domain.Features.Hotels;

namespace Tap.Application.Features.Hotels.UpdateHotel;

public class UpdateHotelCommandHandler : ICommandHandler<UpdateHotelCommand, Result>
{
    private readonly IHotelRepository _hotelRepository;
    private readonly IUserIdentifierProvider _userIdentifierProvider;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateHotelCommandHandler(
        IHotelRepository hotelRepository,
        IUnitOfWork unitOfWork,
        IUserIdentifierProvider userIdentifierProvider
    )
    {
        _hotelRepository = hotelRepository;
        _unitOfWork = unitOfWork;
        _userIdentifierProvider = userIdentifierProvider;
    }

    public async Task<Result> Handle(
        UpdateHotelCommand request,
        CancellationToken cancellationToken
    )
    {
        var maybeHotel = await _hotelRepository.GetByIdAsync(request.Id, cancellationToken);

        if (maybeHotel.HasNoValue)
        {
            return DomainErrors.Hotel.NotFound;
        }

        var hotel = maybeHotel.Value;

        if (hotel.UserId != _userIdentifierProvider.Id)
        {
            return DomainErrors.User.Unauthorized;
        }

        var locationResult = Location.Create(request.Latitude, request.Longitude);

        if (locationResult.IsFailure)
        {
            return locationResult.Error;
        }

        var result = hotel.Update(request.Name, request.Description, locationResult.Value);

        if (result.IsFailure)
        {
            return result.Error;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
