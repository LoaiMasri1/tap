using Tap.Application.Core.Messaging;
using Tap.Contracts.Features.Reviews;
using Tap.Domain.Common.QueryOptions;
using Tap.Domain.Core.Primitives.Maybe;

namespace Tap.Application.Features.Reviews.GetReviews;

public record GetReviewsQuery(
    string? FilterBy,
    string? FilterQuery,
    string SortBy,
    string SortOrder,
    int PageNumber,
    int PageSize
) : IQuery<Maybe<ReviewResponse[]>>, IPageable, IFilterable, ISortable;
