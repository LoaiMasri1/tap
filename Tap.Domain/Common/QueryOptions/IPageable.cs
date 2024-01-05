namespace Tap.Domain.Common.QueryOptions;

public interface IPageable
{
    int PageSize { get; init; }
    int PageNumber { get; init; }
}
