using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;
using Tap.Application.Core.Abstractions.Data;
using Tap.Application.Core.Messaging;
using Tap.Contracts.Features.Cities;
using Tap.Domain.Core.Primitives.Maybe;
using Tap.Domain.Features.Cities;

namespace Tap.Application.Features.Cities.GetCities;

public class GetCitiesQueryHandler : IQueryHandler<GetCitiesQuery, Maybe<CityResponse[]>>
{
    private readonly IDbContext _dbContext;
    private readonly ISieveProcessor _sieveProcessor;

    public GetCitiesQueryHandler(IDbContext dbContext, ISieveProcessor sieveProcessor)
    {
        _dbContext = dbContext;
        _sieveProcessor = sieveProcessor;
    }

    public async Task<Maybe<CityResponse[]>> Handle(
        GetCitiesQuery request,
        CancellationToken cancellationToken
    )
    {
        var sieveModel = new SieveModel
        {
            Filters = request.Filters,
            Sorts = request.Sorts,
            Page = request.Page,
            PageSize = request.PageSize
        };

        var cities = _dbContext.Set<City>().AsNoTracking();

        var citiesPaged = await _sieveProcessor
            .Apply(sieveModel, cities)
            .Select(x => new CityResponse(x.Id, x.Name, x.Description, x.Country))
            .ToArrayAsync(cancellationToken);

        return citiesPaged;
    }
}
