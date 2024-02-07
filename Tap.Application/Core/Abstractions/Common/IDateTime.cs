namespace Tap.Application.Core.Abstractions.Common;

public interface IDateTime
{
    public DateTime UtcNow { get; }
    public string ToShortDateString(DateTime dateTime);
    public string ToLongDateString(DateTime dateTime);
}
