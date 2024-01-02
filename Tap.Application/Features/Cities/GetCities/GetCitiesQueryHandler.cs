using Microsoft.EntityFrameworkCore;
using Tap.Application.Core.Abstractions.Data;
using Tap.Application.Core.Extensions;
using Tap.Application.Core.Messaging;
using Tap.Contracts.Features.Cities;
using Tap.Domain.Core.Primitives.Maybe;
using Tap.Domain.Features.Cities;

namespace Tap.Application.Features.Cities.GetCities;

public class GetCitiesQueryHandler : IQueryHandler<GetCitiesQuery, Maybe<CityResponse[]>>
{
    private readonly IDbContext _dbContext;

    public GetCitiesQueryHandler(IDbContext dbContext) => _dbContext = dbContext;

    public async Task<Maybe<CityResponse[]>> Handle(
        GetCitiesQuery request,
        CancellationToken cancellationToken
    ) =>
        await _dbContext
            .Set<City>()
            .AsNoTracking()
            .Skip(request.PageSize * (request.PageNumber - 1))
            .Take(request.PageSize)
            .OrderBy(request.SortBy, request.SortOrder)
            .Select(c => new CityResponse(c.Id, c.Name, c.Description, c.Country))
            .ToArrayAsync(cancellationToken);
}
