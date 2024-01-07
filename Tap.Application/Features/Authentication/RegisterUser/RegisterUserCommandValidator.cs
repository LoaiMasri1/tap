using FluentValidation;
using Tap.Application.Core.Errors;
using Tap.Application.Core.Extensions;

namespace Tap.Application.Features.Authentication.RegisterUser;

public sealed class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(command => command.Name)
            .NotEmpty()
            .WithError(ValidationErrors.CreateUser.NameIsRequired);

        RuleFor(command => command.Email)
            .NotEmpty()
            .WithError(ValidationErrors.CreateUser.EmailIsRequired);

        RuleFor(command => command.Password)
            .NotEmpty()
            .WithError(ValidationErrors.CreateUser.PasswordIsRequired);

        RuleFor(command => command.Role)
            .NotEmpty()
            .WithError(ValidationErrors.CreateUser.RoleIsRequired);
    }
}
