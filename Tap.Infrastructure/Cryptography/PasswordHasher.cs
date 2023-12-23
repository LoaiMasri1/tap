using System.Security.Cryptography;
using System.Text;
using Tap.Application.Core.Abstractions.Cryptography;
using Tap.Domain.Common.Services;

namespace Tap.Infrastructure.Cryptography;

public class PasswordHasher : IPasswordHasher, IPasswordHashChecker
{
    public bool HashesMatch(string hash, string hashedPassword) =>
        string.Equals(hash, HashPassword(hashedPassword), StringComparison.OrdinalIgnoreCase);

    public string HashPassword(string password)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));

        StringBuilder builder = new();
        foreach (var t in bytes)
        {
            builder.Append(t.ToString("x2"));
        }

        return builder.ToString();
    }
}
