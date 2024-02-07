using FluentValidation;
using Tap.Application.Core.Errors;
using Tap.Application.Core.Extensions;

namespace Tap.Application.Features.Users.UpdateUser;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty().WithError(ValidationErrors.UpdateUser.IdIsRequired);
        RuleFor(c => c.FirstName)
            .NotEmpty()
            .WithError(ValidationErrors.UpdateUser.FirstNameIsRequired);
        RuleFor(c => c.LastName)
            .NotEmpty()
            .WithError(ValidationErrors.UpdateUser.LastNameIsRequired);
    }
}
