using Tap.Application.Core.Abstractions.Common;

namespace Tap.Infrastructure.Common;

public class DateTimeProvider : IDateTime
{
    public DateTime UtcNow => DateTime.UtcNow;
}
