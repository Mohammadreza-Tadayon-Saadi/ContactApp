using Core.Statuses;
using Core.Utilities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Core.CustomResults;

public class ApiResult(bool isSuccess, ApiResultStatusCode statusCode, string? message = null)
{
    public bool IsSuccess { get; set; } = isSuccess;
    public ApiResultStatusCode StatusCode { get; set; } = statusCode;

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string? Message { get; set; } = message ?? statusCode.ToDisplay(DisplayProperty.Name);

    #region Implicit Operator

    public static implicit operator ApiResult(OkResult result)
        => new(true, ApiResultStatusCode.Success);

    public static implicit operator ApiResult(BadRequestResult result)
        => new(false, ApiResultStatusCode.BadRequest);

    public static implicit operator ApiResult(BadRequestObjectResult result)
    {
        var message = result.Value.ToString();
        if (result.Value is SerializableError errors)
        {
            var errorMessages = errors.SelectMany(p => (string[])p.Value).Distinct();
            message = string.Join(" | ", errorMessages);
        }

        return new(false, ApiResultStatusCode.BadRequest, message);
    }

    public static implicit operator ApiResult(ContentResult result)
        => new(true, ApiResultStatusCode.Success, result.Content);

    public static implicit operator ApiResult(NotFoundResult result)
        => new(false, ApiResultStatusCode.NotFound);

    #endregion Implicit Operator
}

public class ApiResult<TData>(bool isSuccess, ApiResultStatusCode statusCode, TData data, string? message = null) : ApiResult(isSuccess, statusCode, message) where TData : class
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public TData Data { get; set; } = data;

    #region Implicit Operator

    public static implicit operator ApiResult<TData>(TData data)
        => new(true, ApiResultStatusCode.Success, data);

    public static implicit operator ApiResult<TData>(OkResult result)
        => new(true, ApiResultStatusCode.Success , null);

    public static implicit operator ApiResult<TData>(OkObjectResult result)
        => new(true, ApiResultStatusCode.Success, (TData)result.Value);

    public static implicit operator ApiResult<TData>(BadRequestResult result)
        => new(false, ApiResultStatusCode.BadRequest, null);

    public static implicit operator ApiResult<TData>(BadRequestObjectResult result)
        => new(false, ApiResultStatusCode.BadRequest, (TData)result.Value, null);

    public static implicit operator ApiResult<TData>(ContentResult result)
        => new(true, ApiResultStatusCode.Success, null, result.Content);

    public static implicit operator ApiResult<TData>(NotFoundResult result)
        => new(false, ApiResultStatusCode.NotFound, null);

    public static implicit operator ApiResult<TData>(NotFoundObjectResult result)
    => new(false, ApiResultStatusCode.NotFound, (TData)result.Value);

    #endregion Implicit Operator
}