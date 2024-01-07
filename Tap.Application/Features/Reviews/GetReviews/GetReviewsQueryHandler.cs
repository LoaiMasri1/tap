using Microsoft.EntityFrameworkCore;
using Tap.Application.Core.Abstractions.Data;
using Tap.Application.Core.Extensions;
using Tap.Application.Core.Messaging;
using Tap.Contracts.Features.Reviews;
using Tap.Domain.Core.Primitives.Maybe;
using Tap.Domain.Features.Reviews;

namespace Tap.Application.Features.Reviews.GetReviews;

public class GetReviewsQueryHandler : IQueryHandler<GetReviewsQuery, Maybe<ReviewResponse[]>>
{
    private readonly IDbContext _dbContext;

    public GetReviewsQueryHandler(IDbContext dbContext) => _dbContext = dbContext;

    public async Task<Maybe<ReviewResponse[]>> Handle(
        GetReviewsQuery request,
        CancellationToken cancellationToken
    ) =>
        await _dbContext
            .Set<Review>()
            .FilterBy(request.FilterBy, request.FilterQuery)
            .OrderBy(request.SortBy, request.SortOrder)
            .Paginate(request.PageNumber, request.PageSize)
            .Select(
                r => new ReviewResponse(r.Id, r.Title, r.Content, r.Rating, r.HotelId, r.UserId)
            )
            .ToArrayAsync(cancellationToken);
}
