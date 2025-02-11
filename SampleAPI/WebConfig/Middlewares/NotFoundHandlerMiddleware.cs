using Core.Utilities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Abstractions;
using WebConfig.Filters;

namespace WebConfig.Middlewares;

public static class NotFoundHandlerMiddlewareExtentions
{
    public static void UseNotFoundHandler(this IApplicationBuilder app)
        => app.UseMiddleware<NotFoundHandlerMiddleware>();
}

public sealed class NotFoundHandlerMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        await _next(context);

        if (context.Response.StatusCode is (>= 400 and < 500))
        {
            var actionDescriptor = context.GetEndpoint()?.Metadata.GetMetadata<ActionDescriptor>();
            if (actionDescriptor is not null)
            {
                var controllerName = actionDescriptor.RouteValues["controller"];
                if (controllerName.HasValue())
                {
                    var controllerType = ApiResultFilterAttribute.FindControllerTypeByName(controllerName);
                    if (controllerType is not null)
                    {
                        if (ApiResultFilterAttribute.HasApiResultFilterAttribute(controllerType))
                            return;

                        var actionName = actionDescriptor.RouteValues["action"];
                        if (actionName.HasValue())
                        {
                            var methodInfo = ApiResultFilterAttribute.FindActionMethodInfoByName(controllerType, actionName);
                            if (methodInfo is not null && ApiResultFilterAttribute.HasApiResultFilterAttribute(methodInfo))
                                return;
                        }
                    }
                }
            }

            context.Response.Redirect("/NotFound");
        }
    }
}