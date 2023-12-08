using Tap.Application.Core.Messaging;
using Tap.Domain.Core.Errors;
using Tap.Domain.Core.Primitives.Result;
using Tap.Application.Core.Abstractions.Data;
using Tap.Contracts.Features.Users;
using Tap.Domain.Features.Users;
using Tap.Application.Core.Abstractions.Cryptography;

namespace Tap.Application.Features.Users.CreateUser;

public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, Result<UserResponse>>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;

    public CreateUserCommandHandler(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IPasswordHasher passwordHasher
    )
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
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

        var hashedPassword = _passwordHasher.HashPassword(password.Value);

        var user = new User(request.Name, email.Value, hashedPassword, request.Role);

        _userRepository.Insert(user);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new UserResponse(user.Id, user.Name, user.Email);
    }
}
