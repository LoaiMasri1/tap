namespace Tap.Contracts.Features.Users
{
    public record UpdateUserRequest(
        int Id,
        string FirstName,
        string LastName,
        string? Password,
        string? ConfirmPassword
    );
}
