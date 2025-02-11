using Core.CustomResults;
using Core.Statuses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Reflection;
using WebConfig.IoC;

namespace WebConfig.Filters;

public class ApiResultFilterAttribute : ActionFilterAttribute
{
    public override void OnResultExecuting(ResultExecutingContext context)
    {
        if (context.Result is OkObjectResult okObjectResult)
        {
            var apiResult = new ApiResult<object>(true, ApiResultStatusCode.Success, okObjectResult.Value);
            context.Result = new OkObjectResult(apiResult) { StatusCode = okObjectResult.StatusCode };
        }
        else if (context.Result is OkResult okResult)
        {
            var apiResult = new ApiResult(true, ApiResultStatusCode.Success);
            context.Result = new ObjectResult(apiResult) { StatusCode = okResult.StatusCode };
        }
        else if (context.Result is BadRequestResult badRequestResult || (context.Result is ObjectResult objResult && objResult.StatusCode == 400 && objResult.Value is null))
        {
            var apiResult = new ApiResult(false, ApiResultStatusCode.BadRequest);
            context.Result = new ObjectResult(apiResult) { StatusCode = 400 };
        }
        else if (context.Result is BadRequestObjectResult badRequestObjectResult)
        {
            var obj = badRequestObjectResult.Value;

            var apiResult = new ApiResult<object>(false, ApiResultStatusCode.BadRequest, obj);
            context.Result = new ObjectResult(apiResult) { StatusCode = badRequestObjectResult.StatusCode };
        }
        else if (context.Result is ContentResult contentResult)
        {
            var apiResult = new ApiResult(true, ApiResultStatusCode.Success, contentResult.Content);
            context.Result = new ObjectResult(apiResult) { StatusCode = contentResult.StatusCode };
        }
        else if (context.Result is NotFoundResult notFoundResult || (context.Result is ObjectResult objRes && objRes.StatusCode == 404))
        {
            var apiResult = new ApiResult(false, ApiResultStatusCode.NotFound);
            context.Result = new ObjectResult(apiResult) { StatusCode = 404 };
        }
        else if (context.Result is NotFoundObjectResult notFoundObjectResult)
        {
            var apiResult = new ApiResult<object>(false, ApiResultStatusCode.NotFound, notFoundObjectResult.Value);
            context.Result = new ObjectResult(apiResult) { StatusCode = notFoundObjectResult.StatusCode };
        }
        else if (context.Result is ObjectResult objectResult && objectResult.StatusCode is null
            && objectResult.Value is not ApiResult)
        {
            var apiResult = new ApiResult<object>(true, ApiResultStatusCode.Success, objectResult.Value);
            context.Result = new ObjectResult(apiResult) { StatusCode = 200 };//200 };
        }
        //else if(context.Result is ObjectResult objResult)
        //{
        //    var apiResult = new ApiResult<object>(true, ApiResultStatusCode.Success, objResult.Value);
        //    context.Result = new ObjectResult(apiResult) { StatusCode = objResult.StatusCode };
        //}
        base.OnResultExecuting(context);
    }

    public static bool HasApiResultFilterAttribute(MemberInfo memberInfo)
    {
        var result = memberInfo.GetCustomAttributes(typeof(ApiResultFilterAttribute), inherit: true).Length != 0;
        return result;
    }

    public static Type? FindControllerTypeByName(string controllerName)
    {
        var controllerType = ServiceInjectionExtentions._presentationAssembly.GetTypes()
            .Where(type => (typeof(Controller).IsAssignableFrom(type) || typeof(ControllerBase).IsAssignableFrom(type)) &&
                type.Name.Equals($"{controllerName}Controller", StringComparison.OrdinalIgnoreCase))
            .FirstOrDefault();

        return controllerType;
    }

    public static MethodInfo? FindActionMethodInfoByName(Type controllerType, string actionName)
        => controllerType.GetMethods().FirstOrDefault(method =>
                            method.Name.Equals(actionName, StringComparison.OrdinalIgnoreCase));
}