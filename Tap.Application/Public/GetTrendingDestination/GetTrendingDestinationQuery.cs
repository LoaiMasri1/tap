using Tap.Application.Core.Messaging;
using Tap.Contracts.Features.Public;
using Tap.Domain.Core.Primitives.Maybe;

namespace Tap.Application.Public.GetTrendingDestination;

public record GetTrendingDestinationQuery(int Limit) : IQuery<Maybe<TrendingDestinationResponse[]>>;
