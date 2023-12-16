using MediatR;
using Tap.Contracts.Features.Authentication;
using Tap.Domain.Common.Services;
using Tap.Domain.Core.Errors;
using Tap.Domain.Core.Primitives.Result;
using Tap.Domain.Features.Users;

namespace Tap.Application.Features.Authentication.LoginUser;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, Result<TokenResponse>>
{
    private readonly IPasswordHashChecker _passwordHashChecker;
    private readonly IJwtProvider _jwtProvider;
    private readonly IUserRepository _userRepository;

    public LoginUserCommandHandler(
        IPasswordHashChecker passwordHashChecker,
        IJwtProvider jwtProvider,
        IUserRepository userRepository
    )
    {
        _passwordHashChecker = passwordHashChecker;
        _jwtProvider = jwtProvider;
        _userRepository = userRepository;
    }

    public async Task<Result<TokenResponse>> Handle(
        LoginUserCommand request,
        CancellationToken cancellationToken
    )
    {
        var emailResult = Email.Create(request.Email);

        if (emailResult.IsFailure)
        {
            return DomainErrors.Authentication.InvalidEmailOrPassword;
        }

        var maybeUser = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);

        if (maybeUser.HasNoValue)
        {
            return DomainErrors.Authentication.InvalidEmailOrPassword;
        }

        var user = maybeUser.Value;

        if (!user.IsActivate)
        {
            return DomainErrors.Authentication.AccountNotActive;
        }

        var passwordValid = user.VerifyPasswordHash(request.Password, _passwordHashChecker);

        if (!passwordValid)
        {
            return DomainErrors.Authentication.InvalidEmailOrPassword;
        }

        var token = _jwtProvider.Create(user);

        return new TokenResponse(token);
    }
}
