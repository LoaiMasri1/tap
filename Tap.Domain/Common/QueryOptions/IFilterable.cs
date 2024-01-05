namespace Tap.Domain.Common.QueryOptions;

public interface IFilterable
{
    string? FilterBy { get; init; }
    string? FilterQuery { get; init; }
}
