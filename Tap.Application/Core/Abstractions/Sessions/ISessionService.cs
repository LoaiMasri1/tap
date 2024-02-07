using Tap.Domain.Common;
using Tap.Domain.Core.Primitives.Maybe;

namespace Tap.Application.Core.Abstractions.Sessions;

public interface ISessionService
{
    Task<string> GetByIdAsync(string sessionId, CancellationToken cancellation = default);
    Task<Maybe<Session>> SaveAsync(int bookingId, CancellationToken cancellation);
}
