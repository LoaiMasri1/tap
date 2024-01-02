namespace Tap.Application.Core.Messaging;

public interface IPagedQuery<out TResponse> : IQuery<TResponse>
{
    int PageSize { get; init; }
    int PageNumber { get; init; }
    string SortBy { get; init; }
    string SortOrder { get; init; }
    string? FilterBy { get; init; }
    string? FilterQuery { get; init; }
}
