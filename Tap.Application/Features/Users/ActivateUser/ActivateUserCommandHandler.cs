using MediatR;
using Tap.Application.Core.Abstractions.Common;
using Tap.Application.Core.Abstractions.Data;
using Tap.Domain.Core.Errors;
using Tap.Domain.Core.Primitives.Result;
using Tap.Domain.Features.Users;

namespace Tap.Application.Features.Users.ActivateUser;

public class ActivateUserCommandHandler : IRequestHandler<ActivateUserCommand, Result<Unit>>
{
    private readonly IDateTime _dateTime;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;


    public ActivateUserCommandHandler(
        IUserRepository userRepository, IUnitOfWork unitOfWork, IDateTime dateTime)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _dateTime = dateTime;
    }

    public async Task<Result<Unit>> Handle(ActivateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByTokenAsync(request.Token, cancellationToken);

        if (user.HasNoValue)
        {
            return DomainErrors.User.NotFound;
        }

        if (user.Value.IsActivate)
        {
            return DomainErrors.User.UserAllReadyActive;
        }

        if (user.Value.ActivationToken?.ExpiredAt < _dateTime.UtcNow)
        {
            return DomainErrors.User.ActivationTokenExpired;
        }

        user.Value.Activate();

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}