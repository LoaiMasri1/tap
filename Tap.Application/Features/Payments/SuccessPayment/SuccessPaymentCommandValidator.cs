using FluentValidation;
using Tap.Application.Core.Errors;
using Tap.Application.Core.Extensions;

namespace Tap.Application.Features.Payments.SuccessPayment;

public class SuccessPaymentCommandValidator : AbstractValidator<SuccessPaymentCommand>
{
    public SuccessPaymentCommandValidator()
    {
        RuleFor(x => x.SessionId)
            .NotEmpty()
            .WithError(ValidationErrors.SuccessPayment.SessionIdIsRequired);
    }
}
