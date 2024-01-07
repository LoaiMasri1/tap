using Microsoft.EntityFrameworkCore;
using Tap.Application.Core.Abstractions.Data;
using Tap.Application.Core.Extensions;
using Tap.Application.Core.Messaging;
using Tap.Contracts.Features.Discounts;
using Tap.Domain.Core.Primitives.Maybe;
using Tap.Domain.Features.Rooms;

namespace Tap.Application.Features.Discounts.GetDiscountsForSpecificHotel;

public class GetDiscountsForSpecificHotelQueryHandler
    : IQueryHandler<GetDiscountsForSpecificHotelQuery, Maybe<FilteredDiscountResponse[]>>
{
    private readonly IDbContext _dbContext;

    public GetDiscountsForSpecificHotelQueryHandler(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Maybe<FilteredDiscountResponse[]>> Handle(
        GetDiscountsForSpecificHotelQuery request,
        CancellationToken cancellationToken
    )
    {
        return await _dbContext
            .Set<Room>()
            .AsNoTracking()
            .Include(r => r.Discounts)
            .Where(r => r.HotelId == request.HotelId)
            .OrderBy(request.SortBy, request.SortOrder)
            .Paginate(request.PageNumber, request.PageSize)
            .SelectMany(r => r.Discounts)
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
