namespace Tap.Domain.Core.Primitives;

public class Error
{
    public static readonly Error None = new(string.Empty, string.Empty);

    public Error(string code, string message) => (Code, Message) = (code, message);

    public string Code { get; }
    public string Message { get; }
    public DateTime Timestamp { get; } = DateTime.UtcNow;

    public static implicit operator string(Error error) => error.Code;

    public override bool Equals(object? obj)
    {
        if (obj is not Error other)
        {
            return false;
        }

        return Code == other.Code && Message == other.Message;
    }

    public Error WithMessage(string message) => new(Code, message);

    public override int GetHashCode() => HashCode.Combine(Code, Message);
}
