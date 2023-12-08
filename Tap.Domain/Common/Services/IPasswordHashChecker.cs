namespace Tap.Domain.Common.Services;

public interface IPasswordHashChecker
{
    bool HashesMatch(string hash, string hashedPassword);
}
