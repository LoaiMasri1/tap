using Microsoft.EntityFrameworkCore;
using Tap.Application.Core.Abstractions.Data;
using Tap.Application.Core.Extensions;
using Tap.Application.Core.Messaging;
using Tap.Contracts.Features.Amenities;
using Tap.Domain.Core.Primitives.Maybe;
using Tap.Domain.Features.Amenities;

namespace Tap.Application.Features.Amenities.GetAmenities;

public class GetAmenitiesQueryHandler : IQueryHandler<GetAmenitiesQuery, Maybe<AmenityResponse[]>>
{
    private readonly IDbContext _dbContext;

    public GetAmenitiesQueryHandler(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Maybe<AmenityResponse[]>> Handle(
        GetAmenitiesQuery request,
        CancellationToken cancellationToken
    ) =>
        await _dbContext
            .Set<Amenity>()
            .AsNoTracking()
            .FilterBy(request.FilterBy, request.FilterQuery)
            .OrderBy(request.SortBy, request.SortOrder)
            .Paginate(request.PageNumber, request.PageSize)
            .Select(x => new AmenityResponse(x.Id, x.Name, x.Description))
            .ToArrayAsync(cancellationToken);
}
