using Tap.Application.Core.Abstractions.Common;
using Tap.Application.Core.Abstractions.Cryptography;
using Tap.Application.Core.Abstractions.Data;
using Tap.Application.Core.Abstractions.Notification;
using Tap.Application.Core.Messaging;
using Tap.Contracts.Emails;
using Tap.Contracts.Features.Users;
using Tap.Domain.Core.Errors;
using Tap.Domain.Core.Primitives.Result;
using Tap.Domain.Features.Users;

namespace Tap.Application.Features.Authentication.RegisterUser;

public class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand, Result<UserResponse>>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IEmailNotificationService _emailNotificationService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTime _dateTime;
    private readonly ITokenGenerator _tokenGenerator;

    public RegisterUserCommandHandler(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IPasswordHasher passwordHasher,
        IEmailNotificationService emailNotificationService,
        IDateTime dateTime,
        ITokenGenerator tokenGenerator
    )
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _emailNotificationService = emailNotificationService;
        _dateTime = dateTime;
        _tokenGenerator = tokenGenerator;
    }

    public async Task<Result<UserResponse>> Handle(
        RegisterUserCommand request,
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

        var token = new Token(_tokenGenerator.Generate(), _dateTime.UtcNow.AddDays(1));

        var hashedPassword = _passwordHasher.HashPassword(password.Value);

        var user = new User(request.Name, email.Value, hashedPassword, request.Role, token);

        var welcomeEmail = new WelcomeEmail(user.Name, user.Email, token.Value);

        await _emailNotificationService.SendWelcomeEmail(welcomeEmail);

        _userRepository.Insert(user);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new UserResponse(user.Id, user.Name, user.Email);
    }
}
