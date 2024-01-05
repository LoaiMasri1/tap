using MediatR;
using Tap.Domain.Core.Primitives.Result;

namespace Tap.Application.Features.Authentication.ActivateUser;

public record ActivateUserCommand(string Token) : IRequest<Result<Unit>>;
