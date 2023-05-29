using PicturesWebApi.Exceptions;
using System.Net;
using System.Text.Json;

namespace PicturesWebApi.Middlewares;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    public ExceptionHandlerMiddleware(RequestDelegate next) =>
        _next = next;

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        var code = HttpStatusCode.InternalServerError;
        var result = string.Empty;

        switch (ex)
        {
            case ImageDownloadException imageDownloadException:
                code = HttpStatusCode.BadRequest;
                result = JsonSerializer.Serialize(imageDownloadException.Message);
                break;
            case InvalidFileExtensionException invalidFileExtensionException:
                code = HttpStatusCode.UnprocessableEntity;
                result = JsonSerializer.Serialize(invalidFileExtensionException.Message);
                break;
            case NotFoundEntityException notFoundEntityException:
                code = HttpStatusCode.NotFound;
                result = JsonSerializer.Serialize(notFoundEntityException.Message);
                break;
            case RootPathException rootPathException:
                code = HttpStatusCode.InternalServerError;
                result = JsonSerializer.Serialize(rootPathException.Message);
                break;
            case SizeOutOfAllowableLimitException sizeOutOfAllowableLimitException:
                code = HttpStatusCode.UnprocessableEntity;
                result = JsonSerializer.Serialize(sizeOutOfAllowableLimitException.Message);
                break;
            case InvalidFormatException invalidFormatException:
                code = HttpStatusCode.BadRequest;
                result = JsonSerializer.Serialize(invalidFormatException.Message);
                break;
        }
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;

        if (result == null)
        {
            result = JsonSerializer.Serialize(new { error = ex.Message });
        }

        return context.Response.WriteAsync(result);
    }
}
