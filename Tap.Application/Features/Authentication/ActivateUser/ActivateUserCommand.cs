using MediatR;
using Tap.Application.Core.Messaging;
using Tap.Domain.Core.Primitives.Result;

namespace Tap.Application.Features.Authentication.ActivateUser;

public record ActivateUserCommand(string Token) : ICommand<Result<Unit>>;
