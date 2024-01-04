using Tap.Application.Core.Messaging;
using Tap.Domain.Core.Primitives.Result;

namespace Tap.Application.Features.Payments.SuccessPayment;

public record SuccessPaymentCommand(string SessionId) : ICommand<Result>;
