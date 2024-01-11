using Microsoft.EntityFrameworkCore;
using Tap.Application.Core.Abstractions.Data;
using Tap.Application.Core.Messaging;
using Tap.Contracts.Features.Cities;
using Tap.Domain.Core.Primitives.Maybe;
using Tap.Domain.Features.Cities;

namespace Tap.Application.Features.Cities.GetCityById;

public class GetCityByIdQueryHandler : IQueryHandler<GetCityByIdQuery, Maybe<CityResponse>>
{
    private readonly IDbContext _dbContext;

    public GetCityByIdQueryHandler(IDbContext dbContext) => _dbContext = dbContext;

    public async Task<Maybe<CityResponse>> Handle(
        GetCityByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        var city = await _dbContext
            .Set<City>()
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (city is null)
        {
            return Maybe<CityResponse>.None;
        }

        return new CityResponse(
            city.Id,
            city.Name,
            city.Description,
            city.Country,
            city.CreatedAtUtc,
            city.UpdatedAtUtc
        );
    }
}
