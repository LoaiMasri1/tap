using FluentValidation;
using Tap.Application.Core.Errors;
using Tap.Application.Core.Extentions;

namespace Tap.Application.Features.Users.CreateUser;

public sealed class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
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
