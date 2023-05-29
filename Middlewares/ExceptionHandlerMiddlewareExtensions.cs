using Microsoft.AspNetCore.Diagnostics;

namespace PicturesWebApi.Middlewares;

public static class ExceptionHandlerMiddlewareExtensions
{
    public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder applicationBuilder)
    {
        return applicationBuilder.UseMiddleware<ExceptionHandlerMiddleware>();
    }
}
