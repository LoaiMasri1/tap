using FluentValidation;
using Tap.Application.Core.Errors;
using Tap.Application.Core.Extentions;

namespace Tap.Application.Features.Authentication.LoginUser;

public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().WithError(ValidationErrors.Login.EmailIsRequired);

        RuleFor(x => x.Password).NotEmpty().WithError(ValidationErrors.Login.PasswordIsRequired);
    }
}
