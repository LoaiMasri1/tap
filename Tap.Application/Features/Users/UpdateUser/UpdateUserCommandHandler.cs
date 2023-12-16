using Tap.Application.Core.Abstractions.Cryptography;
using Tap.Application.Core.Abstractions.Data;
using Tap.Application.Core.Messaging;
using Tap.Application.Features.Authentication;
using Tap.Domain.Core.Errors;
using Tap.Domain.Core.Primitives.Result;
using Tap.Domain.Features.Users;

namespace Tap.Application.Features.Users.UpdateUser;

public class UpdateUserCommandHandler : ICommandHandler<UpdateUserCommand, Result>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserIdentifierProvider _userIdentifierProvider;

    public UpdateUserCommandHandler(
        IUnitOfWork unitOfWork,
        IPasswordHasher passwordHasher,
        IUserRepository userRepository,
        IUserIdentifierProvider userIdentifierProvider
    )
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _userRepository = userRepository;
        _userIdentifierProvider = userIdentifierProvider;
    }

    public async Task<Result> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
    {
        if (command.Id != _userIdentifierProvider.Id)
            return DomainErrors.User.Unauthorized;

        var maybeUser = await _userRepository.GetByIdAsync(command.Id, cancellationToken);

        if (maybeUser.HasNoValue)
            return DomainErrors.User.NotFound;

        var user = maybeUser.Value;

        user.UpdateName(command.FirstName, command.LastName);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        if (
            string.IsNullOrWhiteSpace(command.Password)
            || string.IsNullOrWhiteSpace(command.ConfirmPassword)
        )
        {
            return Result.Success();
        }

        if (command.Password != command.ConfirmPassword)
            return DomainErrors.User.PasswordsDoNotMatch;

        var passwordResult = Password.Create(command.Password);

        if (passwordResult.IsFailure)
            return passwordResult.Error;

        var hashedPassword = _passwordHasher.HashPassword(command.Password);

        var result = user.UpdatePassword(hashedPassword);

        if (result.IsFailure)
            return result.Error;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
