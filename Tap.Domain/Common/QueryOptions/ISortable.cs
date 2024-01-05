namespace Tap.Domain.Common.QueryOptions;

public interface ISortable
{
    string SortBy { get; init; }
    string SortOrder { get; init; }
}
