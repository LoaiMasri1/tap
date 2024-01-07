using Tap.Application.Core.Messaging;
using Tap.Contracts.Features.Public;
using Tap.Domain.Core.Primitives.Maybe;

namespace Tap.Application.Public.GetFeaturedDeals;

public record GetFeaturedDealsQuery(int Limit) : IQuery<Maybe<FeaturedDealResponse[]>>;
