using Tap.Application.Core.Messaging;
using Tap.Domain.Core.Errors;
using Tap.Domain.Core.Primitives.Result;
using Tap.Application.Core.Abstractions.Data;
using Tap.Contracts.Features.Users;
using Tap.Domain.Features.Users;

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
        var email = Email.Create(request.Email);
        var password = Password.Create(request.Password);

        var result = Result.Combine(email, password);

        if (result.IsFailure)
        {
            return result.Error;
        }

        if (await _userRepository.IsEmailUniqueAsync(email.Value, cancellationToken))
        {
            return DomainErrors.User.DuplicateEmail;
        }

        var user = new User(request.Name, email.Value, password.Value, request.Role);

        _userRepository.Insert(user);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new UserResponse(user.Id, user.Name, user.Email);
    }
}
