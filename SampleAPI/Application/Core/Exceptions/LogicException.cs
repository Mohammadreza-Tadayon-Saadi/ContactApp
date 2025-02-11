namespace Application.Core.Exceptions;

public class LogicException : AppException
{
    public LogicException() 
        : base(ApiResultStatusCode.LogicException)
    {
    }

    public LogicException(string message) 
        : base(ApiResultStatusCode.LogicException, message)
    {
    }

    public LogicException(object additionalData) 
        : base(ApiResultStatusCode.LogicException, additionalData)
    {
    }

    public LogicException(string message, object additionalData) 
        : base(ApiResultStatusCode.LogicException, message, additionalData)
    {
    }

    public LogicException(string message, Exception exception)
        : base(ApiResultStatusCode.LogicException, message, exception)
    {
    }

    public LogicException(string message, Exception exception, object additionalData)
        : base(ApiResultStatusCode.LogicException, message, exception, additionalData)
    {
    }
}