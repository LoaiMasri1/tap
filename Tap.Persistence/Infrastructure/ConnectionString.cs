namespace Tap.Persistence.Infrastructure;

public class ConnectionString
{
    public const string SectionName = "Database";

    public string? Value { get; set; }

    public ConnectionString(string? value) => Value = value;

    public static implicit operator string?(ConnectionString connectionString) =>
        connectionString.Value;
}
