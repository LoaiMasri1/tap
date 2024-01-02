using Tap.Domain.Core.Primitives;

namespace Tap.Domain.Core.Exceptions;

public class PropertyNotFoundException : DomainException
{
    public PropertyNotFoundException(string message)
        : base(new Error("Error.PropertyNotFound", message)) { }
}
