namespace Tap.Contracts.Features.Users;

public record CreateUserRequest(string FirstName, string LastName, string Email, string Password);
