namespace Tap.Domain.Core.Primitives.Result;

public class Result
{
    protected internal Result(bool isSuccess, Error error) =>
        (IsSuccess, Error) = (isSuccess, error);

    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error Error { get; }

    public static Result Success() => new(true, Error.None);

    public static Result Failure(Error error) => new(false, error);

    public static implicit operator Result(Error error) => Failure(error);

    public static Result<TIn> Create<TIn>(TIn value) => new(value, true, Error.None);

    public static Result Combine(params Result[] results)
    {
        foreach (var result in results)
        {
            if (result.IsFailure)
            {
                return result;
            }
        }

        return Success();
    }

    public static Result Combine<TIn>(params Result<TIn>[] results)
    {
        foreach (var result in results)
        {
            if (result.IsFailure)
            {
                return result;
            }
        }

        return Success();
    }
}
