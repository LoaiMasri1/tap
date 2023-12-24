using Tap.Application.Core.Abstractions.Data;
using Tap.Application.Core.Messaging;
using Tap.Application.Features.Authentication;
using Tap.Contracts.Features.Amenities;
using Tap.Domain.Core.Errors;
using Tap.Domain.Core.Primitives.Result;
using Tap.Domain.Features.Amenities;
using Tap.Domain.Features.Users;

namespace Tap.Application.Features.Amenities.UpdateAmenity;

public class UpdateAmenityCommandHandler
    : ICommandHandler<UpdateAmenityCommand, Result<AmenityResponse>>
{
    private readonly IAmenityRepository _amenityRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserIdentifierProvider _userIdentifierProvider;

    public UpdateAmenityCommandHandler(
        IUnitOfWork unitOfWork,
        IAmenityRepository amenityRepository,
        IUserIdentifierProvider userIdentifierProvider
    )
    {
        _unitOfWork = unitOfWork;
        _amenityRepository = amenityRepository;
        _userIdentifierProvider = userIdentifierProvider;
    }

    public async Task<Result<AmenityResponse>> Handle(
        UpdateAmenityCommand request,
        CancellationToken cancellationToken
    )
    {
        var userRole = _userIdentifierProvider.Role;
        if (userRole != UserRole.Admin)
        {
            return DomainErrors.User.Unauthorized;
        }

        var maybeAmenity = await _amenityRepository.GetByIdAsync(request.Id, cancellationToken);
        if (maybeAmenity.HasNoValue)
        {
            return DomainErrors.Amenity.NotFound;
        }

        var amenity = maybeAmenity.Value;

        var result = amenity.Update(request.Name, request.Description);

        if (result.IsFailure)
        {
            return result.Error;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new AmenityResponse(amenity.Id, amenity.Name, amenity.Description);
    }
}
