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
    private readonly IUserContext _userContext;

    public CreateAmenityCommandHandler(
        IAmenityRepository amenityRepository,
        IUnitOfWork unitOfWork,
        IAmenityService amenityService,
        IUserContext userContext
    )
    {
        _amenityRepository = amenityRepository;
        _unitOfWork = unitOfWork;
        _amenityService = amenityService;
        _userContext = userContext;
    }

    public async Task<Result<AmenityResponse>> Handle(
        CreateAmenityCommand request,
        CancellationToken cancellationToken
    )
    {
        var userRole = _userContext.Role;

        if (userRole != UserRole.Admin)
        {
            return DomainErrors.User.Unauthorized;
        }

        var userId = _userContext.Id;

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
