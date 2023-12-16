using FluentValidation;
using Tap.Application.Core.Errors;
using Tap.Application.Core.Extentions;

namespace Tap.Application.Features.Users.ActivateUser;

public class ActivateUserCommandValidator : AbstractValidator<ActivateUserCommand>
{
    public ActivateUserCommandValidator()
    {
        RuleFor(x => x.Token).NotEmpty().WithError(ValidationErrors.ActivateUser.TokenIsRequired);
    }
}