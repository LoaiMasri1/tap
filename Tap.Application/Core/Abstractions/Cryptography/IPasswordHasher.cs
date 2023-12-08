namespace Tap.Application.Core.Abstractions.Cryptography;

public interface IPasswordHasher
{
    string HashPassword(string password);
}
