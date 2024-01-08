using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;
using Tap.Application.Core.Abstractions.Data;
using Tap.Application.Core.Messaging;
using Tap.Contracts.Features.Reviews;
using Tap.Domain.Core.Primitives.Maybe;
using Tap.Domain.Features.Reviews;

namespace Tap.Application.Features.Reviews.GetReviews;

public class GetReviewsQueryHandler : IQueryHandler<GetReviewsQuery, Maybe<ReviewResponse[]>>
{
    private readonly IDbContext _dbContext;
    private readonly ISieveProcessor _sieveProcessor;

    public GetReviewsQueryHandler(IDbContext dbContext, ISieveProcessor sieveProcessor)
    {
        _dbContext = dbContext;
        _sieveProcessor = sieveProcessor;
    }

    public async Task<Maybe<ReviewResponse[]>> Handle(
        GetReviewsQuery request,
        CancellationToken cancellationToken
    )
    {
        var reviews = _dbContext.Set<Review>().AsNoTracking();

        var sieveModel = new SieveModel
        {
            Filters = request.Filters,
            Sorts = request.Sorts,
            Page = request.Page,
            PageSize = request.PageSize
        };

        return await _sieveProcessor
            .Apply(sieveModel, reviews)
            .Select(
                r => new ReviewResponse(r.Id, r.Title, r.Content, r.Rating, r.HotelId, r.UserId)
            )
            .ToArrayAsync(cancellationToken);
    }
}
