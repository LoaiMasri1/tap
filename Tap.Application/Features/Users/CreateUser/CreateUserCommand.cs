using Tap.Application.Core.Messaging;
using Tap.Contracts.Features.Users;
using Tap.Domain.Core.Primitives.Result;

namespace Tap.Application.Features.Users.CreateUser;

public record CreateUserCommand(string Name, string Email, string Password)
    : ICommand<Result<UserResponse>>;
