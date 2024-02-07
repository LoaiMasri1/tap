using Tap.Application.Core.Messaging;
using Tap.Contracts.Features.Cities;
using Tap.Domain.Common.QueryOptions;
using Tap.Domain.Core.Primitives.Maybe;

namespace Tap.Application.Features.Cities.GetCities;

public record GetCitiesQuery(string Filters, string Sorts, int Page, int PageSize)
    : IQuery<Maybe<CityResponse[]>>,
        IPageable,
        ISortable,
        IFilterable;
