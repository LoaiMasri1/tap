using System.Net;
using System.Text.Json;
using Tap.Application.Core.Exceptions;
using Tap.Domain.Core.Errors;
using Tap.Domain.Core.Exceptions;
using Tap.Domain.Core.Primitives;
using Tap.Services.Api.Contracts;

namespace Tap.Services.Api.Middlewares;

public class ExceptionHandlerMiddlware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        (HttpStatusCode statusCode, IReadOnlyCollection<Error> errors) = GetHttpStatusCodeAndError(
            ex
        );

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        string response = JsonSerializer.Serialize(
            new ApiResponse { Errors = errors, StatusCode = statusCode }
        );

        await Console.Out.WriteLineAsync($"{ex.Message}:{ex.StackTrace}");

        await context.Response.WriteAsync(response);
    }

    private static (
        HttpStatusCode statusCode,
        IReadOnlyCollection<Error> errors
    ) GetHttpStatusCodeAndError(Exception exception)
    {
        Console.WriteLine(exception.Message);

        return exception switch
        {
            ValidationException validationException
                => (HttpStatusCode.BadRequest, validationException.Errors),
            DomainException domainException
                => (HttpStatusCode.BadRequest, new[] { domainException.Error }),
            _ => (HttpStatusCode.InternalServerError, new[] { DomainErrors.General.ServerError })
        };
    }
}
