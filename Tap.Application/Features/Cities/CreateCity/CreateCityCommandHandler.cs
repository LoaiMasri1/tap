using Tap.Application.Core.Abstractions.Data;
using Tap.Application.Core.Messaging;
using Tap.Application.Features.Authentication;
using Tap.Contracts.Features.Cities;
using Tap.Domain.Core.Errors;
using Tap.Domain.Core.Primitives.Result;
using Tap.Domain.Features.Cities;
using Tap.Domain.Features.Users;

namespace Tap.Application.Features.Cities.CreateCity;

public class CreateCityCommandHandler : ICommandHandler<CreateCityCommand, Result<CityResponse>>
{
    private readonly ICityRepository _cityRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserIdentifierProvider _userIdentifierProvider;

    public CreateCityCommandHandler(
        ICityRepository cityRepository,
        IUnitOfWork unitOfWork,
        IUserIdentifierProvider userIdentifierProvider
    )
    {
        _cityRepository = cityRepository;
        _unitOfWork = unitOfWork;
        _userIdentifierProvider = userIdentifierProvider;
    }

    public async Task<Result<CityResponse>> Handle(
        CreateCityCommand request,
        CancellationToken cancellationToken
    )
    {
        var userRole = _userIdentifierProvider.Role;
        if (userRole != UserRole.Admin)
        {
            return DomainErrors.User.Unauthorized;
        }

        var maybeCity = await _cityRepository.GetByNameAsync(request.Name, cancellationToken);

        if (maybeCity.HasValue)
        {
            return DomainErrors.City.AlreadyExists;
        }

        var city = new City(request.Name, request.Description, request.Country);

        _cityRepository.Insert(city);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new CityResponse(city.Id, city.Name, city.Description, city.Country);
    }
}
