using Tap.Application.Core.Messaging;
using Tap.Contracts.Features.Users;
using Tap.Domain.Core.Primitives.Maybe;

namespace Tap.Application.Features.Users.GetRecentVisits;

public record GetRecentVisitsQuery(int Limit) : IQuery<Maybe<RecentVisitsResponse[]>>;
