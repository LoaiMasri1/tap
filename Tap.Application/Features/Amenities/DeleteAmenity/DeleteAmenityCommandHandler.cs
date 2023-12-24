using Tap.Application.Core.Abstractions.Data;
using Tap.Application.Core.Messaging;
using Tap.Application.Features.Authentication;
using Tap.Domain.Core.Errors;
using Tap.Domain.Core.Primitives.Result;
using Tap.Domain.Features.Amenities;
using Tap.Domain.Features.Users;

namespace Tap.Application.Features.Amenities.DeleteAmenity;

public class DeleteAmenityCommandHandler : ICommandHandler<DeleteAmenityCommand, Result>
{
    private readonly IAmenityRepository _amenityRepository;
    private readonly IUserIdentifierProvider _userIdentifierProvider;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteAmenityCommandHandler(
        IUnitOfWork unitOfWork,
        IAmenityRepository amenityRepository,
        IUserIdentifierProvider userIdentifierProvider
    )
    {
        _unitOfWork = unitOfWork;
        _amenityRepository = amenityRepository;
        _userIdentifierProvider = userIdentifierProvider;
    }

    public async Task<Result> Handle(
        DeleteAmenityCommand request,
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
        _amenityRepository.Remove(amenity);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
