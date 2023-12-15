using MediatR;
using Tap.Domain.Core.Primitives.Result;

namespace Tap.Application.Features.Users.ActivateUser;

public record ActivateUserCommand (string Token) : IRequest<Result<Unit>>;