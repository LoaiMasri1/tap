using Tap.Application.Core.Messaging;
using Tap.Contracts.Features.Cities;
using Tap.Domain.Core.Primitives.Maybe;

namespace Tap.Application.Features.Cities.GetCityById;

public record GetCityByIdQuery(int Id) : IQuery<Maybe<CityResponse>>;
