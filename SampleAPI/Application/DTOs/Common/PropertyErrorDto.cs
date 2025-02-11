using Application.Core.Exceptions;
using SharpCompress;

namespace Application.DTOs.Common;

public record PropertyErrorDto
{
    private PropertyErrorDto(string key, string errorMessage)
    {
        Key = key;
        ErrorMessage = errorMessage;
    }

    public string Key { get; set; }
    public string ErrorMessage { get; set; }

    public static PropertyErrorDto Create(string key, string errorMessage)
    {
        if (!key.HasValue())
            throw new AppException(nameof(key), "The error can not be null or empty!");
        if (!errorMessage.HasValue())
            throw new AppException(nameof(errorMessage), "The error can not be null or empty!");

        return new PropertyErrorDto(key, errorMessage);
    }

    public static PropertyErrorDto Create(Error error)
    {
        if (!error.Key.HasValue())
            throw new AppException(nameof(error.Key), "The error can not be null or empty!");
        if (!error.Message.HasValue())
            throw new AppException(nameof(error.Message), "The error can not be null or empty!");

        return new PropertyErrorDto(error.Key, error.Message);
    }

    public static IEnumerable<object> CreateForView(Error error)
    {
        if (!error.Key.HasValue())
            throw new AppException(nameof(error.Key),"The error can not be null or empty!");
        if (!error.Message.HasValue())
            throw new AppException(nameof(error.Message), "The error can not be null or empty!");
        return (new { error.Key, ErrorMessage = error.Message }).AsEnumerable();
    }
}