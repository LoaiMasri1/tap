namespace Tap.Domain.Core.Primitives;

public class Error
{
    public static readonly Error None = new(string.Empty, string.Empty);
    public static readonly Error NullValue = new("Error.NullValue", "Value cannot be null");

    public Error(string code, string message) => (Code, Message) = (code, message);

    public string Code { get; }
    public string Message { get; }
    public DateTime Timestamp { get; } = DateTime.UtcNow;

    public static implicit operator string(Error error) => error.Code;
}
