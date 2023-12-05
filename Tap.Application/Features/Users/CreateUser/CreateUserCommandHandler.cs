using Tap.Application.Core.Messaging;
using Tap.Domain.Repositories;
using Tap.Domain.Core.Errors;
using Tap.Domain.Core.Primitives.Result;
using Tap.Application.Core.Abstractions.Data;
using Tap.Domain.Entities;
using Tap.Contracts.Features.Users;

namespace Tap.Application.Features.Users.CreateUser;

public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, Result<UserResponse>>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<UserResponse>> Handle(
        CreateUserCommand request,
        CancellationToken cancellationToken
    )
    {
        if (await _userRepository.IsEmailUniqueAsync(request.Email, cancellationToken))
        {
            return DomainErrors.User.DuplicateEmail;
        }

        var user = new User(request.Name, request.Email, request.Password);

        _userRepository.Insert(user);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new UserResponse(user.Id, user.Name, user.Email);
    }
}
