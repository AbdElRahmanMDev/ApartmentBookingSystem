using Domain.Abstraction;

public class Result
{
    protected internal Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None)
            throw new InvalidOperationException();

        if (!isSuccess && error == Error.None)
            throw new InvalidOperationException();

        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error Error { get; }

    public static Result Success() => new Result(true, Error.None);
    public static Result Failure(Error error) => new Result(false, error);

    public static Result<T> Success<T>(T value) =>
        new Result<T>(true, Error.None, value);

    public static Result<T> Failure<T>(Error error) =>
        new Result<T>(false, error, default!);
}


public class Result<T> : Result
{
    private readonly T _value;

    internal Result(bool isSuccess, Error error, T value)
        : base(isSuccess, error)
    {
        _value = value;
    }

    public static implicit operator Result<T>(T value)
    {
        return Success(value);
    }

    public T Value =>
        IsSuccess ? _value :
        throw new InvalidOperationException("Cannot access Value for a failed result.");
}
