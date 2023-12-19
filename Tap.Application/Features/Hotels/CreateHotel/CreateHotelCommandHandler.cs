using Tap.Application.Core.Abstractions.Data;
using Tap.Application.Core.Messaging;
using Tap.Application.Features.Authentication;
using Tap.Contracts.Features.Hotels;
using Tap.Domain.Core.Errors;
using Tap.Domain.Core.Primitives.Result;
using Tap.Domain.Features.Cities;
using Tap.Domain.Features.Hotels;
using Tap.Domain.Features.Users;

namespace Tap.Application.Features.Hotels.CreateHotel;

public class CreateHotelCommandHandler : ICommandHandler<CreateHotelCommand, Result<HotelResponse>>
{
    private readonly ICityRepository _cityRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserIdentifierProvider _userIdentifierProvider;

    public CreateHotelCommandHandler(
        ICityRepository cityRepository,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IUserIdentifierProvider userIdentifierProvider
    )
    {
        _cityRepository = cityRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _userIdentifierProvider = userIdentifierProvider;
    }

    public async Task<Result<HotelResponse>> Handle(
        CreateHotelCommand command,
        CancellationToken cancellationToken
    )
    {
        var userRole = _userIdentifierProvider.Role;
        if (userRole != UserRole.Admin)
        {
            return DomainErrors.User.Unauthorized;
        }

        var maybeCity = await _cityRepository.GetByIdAsync(command.CityId, cancellationToken);
        if (maybeCity.HasNoValue)
        {
            return DomainErrors.City.NotFound;
        }

        var city = maybeCity.Value;

        var maybeUser = await _userRepository.GetByIdAsync(
            _userIdentifierProvider.Id,
            cancellationToken
        );
        if (maybeUser.HasNoValue)
        {
            return DomainErrors.User.NotFound;
        }

        var locationResult = Location.Create(command.Latitude, command.Longitude);

        if (locationResult.IsFailure)
        {
            return locationResult.Error;
        }

        var hotel = new Hotel(
            command.Name,
            command.Description,
            locationResult.Value,
            maybeUser.Value
        );

        city.AddHotel(hotel);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new HotelResponse(
            hotel.Id,
            hotel.Name,
            hotel.Description,
            hotel.Location.Latitude,
            hotel.Location.Longitude
        );
    }
}
