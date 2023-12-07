using Tap.Domain.Core.Primitives;

namespace Tap.Domain.Core.Exceptions;

public class DomainException : Exception
{
    public DomainException(Error error)
        : base(error.Message) => Error = error;

    public Error Error { get; }
}
