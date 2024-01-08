using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;
using Tap.Application.Core.Abstractions.Data;
using Tap.Application.Core.Messaging;
using Tap.Contracts.Features.Discounts;
using Tap.Domain.Core.Primitives.Maybe;
using Tap.Domain.Features.Rooms;

namespace Tap.Application.Features.Discounts.GetDiscountsForSpecificHotel;

public class GetDiscountsForSpecificHotelQueryHandler
    : IQueryHandler<GetDiscountsForSpecificHotelQuery, Maybe<FilteredDiscountResponse[]>>
{
    private readonly IDbContext _dbContext;
    private readonly ISieveProcessor _sieveProcessor;

    public GetDiscountsForSpecificHotelQueryHandler(
        IDbContext dbContext,
        ISieveProcessor sieveProcessor
    )
    {
        _dbContext = dbContext;
        _sieveProcessor = sieveProcessor;
    }

    public async Task<Maybe<FilteredDiscountResponse[]>> Handle(
        GetDiscountsForSpecificHotelQuery request,
        CancellationToken cancellationToken
    )
    {
        var discounts = _dbContext
            .Set<Room>()
            .AsNoTracking()
            .Include(r => r.Discounts)
            .Where(r => r.HotelId == request.HotelId)
            .SelectMany(r => r.Discounts);

        var sieveModel = new SieveModel
        {
            Filters = request.Filters,
            Sorts = request.Sorts,
            Page = request.Page,
            PageSize = request.PageSize,
        };

        return await _sieveProcessor
            .Apply(sieveModel, discounts)
            .Select(
                d =>
                    new FilteredDiscountResponse(
                        d.Id,
                        d.Name,
                        d.Description,
                        d.DiscountPercentage,
                        d.StartDate,
                        d.EndDate
                    )
            )
            .ToArrayAsync(cancellationToken);
    }
}
