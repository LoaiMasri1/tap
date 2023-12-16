using System.Windows.Input;
using Tap.Application.Core.Messaging;
using Tap.Domain.Core.Primitives;
using Tap.Domain.Core.Primitives.Result;

namespace Tap.Application.Features.Users.UpdateUser;

public record UpdateUserCommand(
    int Id,
    string FirstName,
    string LastName,
    string? Password,
    string? ConfirmPassword
) : ICommand<Result>;
