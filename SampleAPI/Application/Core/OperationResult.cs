namespace Application.Core;

public class OperationResult
{
    protected internal OperationResult(bool isSuccess, Error error)
    {
        var isNone = error.Equals(Error.None);
        if (isSuccess && !isNone)
            throw new InvalidOperationException();

        if (!isSuccess && isNone)
            throw new InvalidOperationException();

        IsSuccess = isSuccess;
        Error = error;
    }

    /// <summary>
    /// Gets a value indicating whether the result is a success result.
    /// </summary>
    public bool IsSuccess { get; }

    /// <summary>
    /// Gets a value indicating whether the result is a failure result.
    /// </summary>
    public bool IsFailure => !IsSuccess;

    /// <summary>
    /// Gets the error.
    /// </summary>
    public Error Error { get; }

    /// <summary>
    /// Returns a success <see cref="OperationResult"/>.
    /// </summary>
    /// <returns>A new instance of <see cref="OperationResult"/> with the success flag set.</returns>
    public static OperationResult Success() => new OperationResult(true, Error.None);

    /// <summary>
    /// Returns a success <see cref="OperationResult{TValue}"/> with the specified value.
    /// </summary>
    /// <typeparam name="TValue">The result type.</typeparam>
    /// <param name="value">The result value.</param>
    /// <returns>A new instance of <see cref="OperationResult{TValue}"/> with the success flag set.</returns>
    public static OperationResult<TValue> Success<TValue>(TValue value) => new OperationResult<TValue>(value, true, Error.None);

    /// <summary>
    /// Creates a new <see cref="OperationResult{TValue}"/> with the specified nullable value and the specified error.
    /// </summary>
    /// <typeparam name="TValue">The result type.</typeparam>
    /// <param name="value">The result value.</param>
    /// <param name="error">The error in case the value is null.</param>
    /// <returns>A new instance of <see cref="OperationResult{TValue}"/> with the specified value or an error.</returns>
    public static OperationResult<TValue> Create<TValue>(TValue value, Error error)
        where TValue : class
        => value is null ? Failure<TValue>(error) : Success(value);

    /// <summary>
    /// Returns a failure <see cref="OperationResult"/> with the specified error.
    /// </summary>
    /// <param name="error">The error.</param>
    /// <returns>A new instance of <see cref="OperationResult"/> with the specified error and failure flag set.</returns>
    public static OperationResult Failure(Error error) => new OperationResult(false, error);

    /// <summary>
    /// Returns a failure <see cref="OperationResult{TValue}"/> with the specified error.
    /// </summary>
    /// <typeparam name="TValue">The result type.</typeparam>
    /// <param name="error">The error.</param>
    /// <returns>A new instance of <see cref="OperationResult{TValue}"/> with the specified error and failure flag set.</returns>
    /// <remarks>
    /// We're purposefully ignoring the nullable assignment here because the API will never allow it to be accessed.
    /// The value is accessed through a method that will throw an exception if the result is a failure result.
    /// </remarks>
    public static OperationResult<TValue> Failure<TValue>(Error error) => new OperationResult<TValue>(default!, false, error);

    public static IEnumerable<OperationResult> CheckFailure(params OperationResult[] results)
    {
        foreach (var result in results)
            if (result.IsFailure)
                yield return result;
    }

    public static IEnumerable<OperationResult<TValue>> CheckFailure<TValue>(params OperationResult<TValue>[] results)
    {
        foreach (var result in results)
            if (result.IsFailure)
                yield return result;
    }
}

public class OperationResult<TValue> : OperationResult
{
    private readonly TValue _value;

    protected internal OperationResult(TValue value, bool isSuccess, Error error)
        : base(isSuccess, error)
        =>  _value = value;

    public TValue Value => IsSuccess
        ? _value
        : throw new InvalidOperationException("The Value Of A Failure Result Can Not Be Accessed.");

    public static implicit operator OperationResult<TValue>(TValue value) => Success(value);
}