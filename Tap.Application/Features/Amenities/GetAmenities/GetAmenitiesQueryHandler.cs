using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;
using Tap.Application.Core.Abstractions.Data;
using Tap.Application.Core.Messaging;
using Tap.Contracts.Features.Amenities;
using Tap.Domain.Core.Primitives.Maybe;
using Tap.Domain.Features.Amenities;

namespace Tap.Application.Features.Amenities.GetAmenities;

public class GetAmenitiesQueryHandler : IQueryHandler<GetAmenitiesQuery, Maybe<AmenityResponse[]>>
{
    private readonly IDbContext _dbContext;
    private readonly ISieveProcessor _sieveProcessor;

    public GetAmenitiesQueryHandler(IDbContext dbContext, ISieveProcessor sieveProcessor)
    {
        _dbContext = dbContext;
        _sieveProcessor = sieveProcessor;
    }

    public async Task<Maybe<AmenityResponse[]>> Handle(
        GetAmenitiesQuery request,
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

        var amenities = _dbContext.Set<Amenity>().AsNoTracking();

        return await _sieveProcessor
            .Apply(sieveModel, amenities)
            .Select(x => new AmenityResponse(x.Id, x.Name, x.Description))
            .ToArrayAsync(cancellationToken);
    }
}
