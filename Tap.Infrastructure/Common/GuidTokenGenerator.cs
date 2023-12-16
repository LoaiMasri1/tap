using Tap.Application.Core.Abstractions.Common;

namespace Tap.Infrastructure.Common;

public class GuidTokenGenerator : ITokenGenerator
{
    public string Generate() => Guid.NewGuid().ToString();
}
