using Tap.Application.Core.Abstractions.Data;
using Tap.Application.Core.Messaging;
using Tap.Application.Features.Authentication;
using Tap.Contracts.Features.Amenities;
using Tap.Domain.Core.Errors;
using Tap.Domain.Core.Primitives.Result;
using Tap.Domain.Features.Amenities;
using Tap.Domain.Features.Users;

namespace Tap.Application.Features.Amenities.CreateAmenity;

public class CreateAmenityCommandHandler
    : ICommandHandler<CreateAmenityCommand, Result<AmenityResponse>>
{
    private readonly IAmenityRepository _amenityRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAmenityService _amenityService;
    private readonly IUserIdentifierProvider _userIdentifierProvider;

    public CreateAmenityCommandHandler(
        IAmenityRepository amenityRepository,
        IUnitOfWork unitOfWork,
        IAmenityService amenityService,
        IUserIdentifierProvider userIdentifierProvider
    )
    {
        _amenityRepository = amenityRepository;
        _unitOfWork = unitOfWork;
        _amenityService = amenityService;
        _userIdentifierProvider = userIdentifierProvider;
    }

    public async Task<Result<AmenityResponse>> Handle(
        CreateAmenityCommand request,
        CancellationToken cancellationToken
    )
    {
        var userRole = _userIdentifierProvider.Role;

        if (userRole != UserRole.Admin)
        {
            return DomainErrors.User.Unauthorized;
        }

        var userId = _userIdentifierProvider.Id;

        var result = await _amenityService.CheckAmenityTypeAndUserOwnerShipAsync(
            userId,
            request.TypeId,
            request.Type,
            cancellationToken
        );

        if (result.IsFailure)
        {
            return result.Error;
        }

        var amenity = Amenity.Create(
            request.Name,
            request.Description,
            request.Type,
            request.TypeId
        );

        _amenityRepository.Insert(amenity);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new AmenityResponse(amenity.Id, amenity.Name, amenity.Description);
    }
}
