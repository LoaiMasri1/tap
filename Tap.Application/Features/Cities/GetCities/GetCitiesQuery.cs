using Tap.Application.Core.Messaging;
using Tap.Contracts.Features.Cities;
using Tap.Domain.Core.Primitives.Maybe;

namespace Tap.Application.Features.Cities.GetCities;

public record GetCitiesQuery(int PageSize, int PageNumber, string SortBy, string SortOrder)
    : IPagedQuery<Maybe<CityResponse[]>>;
