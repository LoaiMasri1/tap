using Tap.Application.Core.Messaging;
using Tap.Contracts.Features.Reviews;
using Tap.Domain.Common.QueryOptions;
using Tap.Domain.Core.Primitives.Maybe;

namespace Tap.Application.Features.Reviews.GetReviews;

public record GetReviewsQuery(string Filters, string Sorts, int Page, int PageSize)
    : IQuery<Maybe<ReviewResponse[]>>,
        IPageable,
        IFilterable,
        ISortable;
