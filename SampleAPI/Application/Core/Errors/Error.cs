namespace Application.Core.Errors;

public sealed class Error : ValueObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Error"/> class.
    /// </summary>
    /// <param name="key">The error code.</param>
    /// <param name="message">The error message.</param>
    public Error(string key, string message, ErrorType errorType = ErrorType.PropertyValidation)
    {
        Key = key;
        Message = message;
        ErrorType = errorType;
    }

    /// <summary>
    /// Gets the error code.
    /// </summary>
    public string Key { get; }

    /// <summary>
    /// Gets the error message.
    /// </summary>
    public string Message { get; }

    /// <summary>
    /// Gets the error type.
    /// </summary>
    public ErrorType ErrorType { get; }

    public static implicit operator string(Error error) => error?.Key ?? string.Empty;

    /// <inheritdoc />
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Key;
        yield return Message;
        yield return ErrorType;
    }

    /// <summary>
    /// Gets the empty error instance.
    /// </summary>
    internal static Error None => new Error(string.Empty, string.Empty);
}

public enum ErrorType
{
    PropertyValidation,
    Entity,
    ValueObject,
    Server,
}