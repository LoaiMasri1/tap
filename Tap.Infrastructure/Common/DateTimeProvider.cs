using Tap.Application.Core.Abstractions.Common;

namespace Tap.Infrastructure.Common;

public class DateTimeProvider : IDateTime
{
    public DateTime UtcNow => DateTime.UtcNow;

    public string ToShortDateString(DateTime dateTime) => dateTime.ToShortDateString();

    public string ToLongDateString(DateTime dateTime) => dateTime.ToLongDateString();
}
