namespace Tap.Application.Core.Messaging;

public interface IPageable
{
    int PageSize { get; init; }
    int PageNumber { get; init; }
}

public interface ISortable
{
    string SortBy { get; init; }
    string SortOrder { get; init; }
}

public interface IFilterable
{
    string? FilterBy { get; init; }
    string? FilterQuery { get; init; }
}
