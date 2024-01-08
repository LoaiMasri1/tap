namespace Tap.Domain.Common.QueryOptions;

public interface IPageable
{
    int Page { get; init; }
    int PageSize { get; init; }
}
