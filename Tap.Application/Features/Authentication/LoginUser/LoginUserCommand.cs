using MediatR;
using Tap.Contracts.Features.Authentication;
using Tap.Domain.Core.Primitives.Result;

namespace Tap.Application.Features.Authentication.LoginUser;

public record LoginUserCommand(string Email, string Password) : IRequest<Result<TokenResponse>>;
