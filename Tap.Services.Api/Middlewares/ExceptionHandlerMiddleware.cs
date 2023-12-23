using System.Net;
using Tap.Application.Core.Exceptions;
using Tap.Domain.Core.Errors;
using Tap.Domain.Core.Exceptions;
using Tap.Domain.Core.Primitives;
using Tap.Services.Api.Contracts;

namespace Tap.Services.Api.Middlewares;

public class ExceptionHandlerMiddleware : IMiddleware
{
    private readonly ILogger<ExceptionHandlerMiddleware> _logger;

    public ExceptionHandlerMiddleware(ILogger<ExceptionHandlerMiddleware> logger) =>
        _logger = logger;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        var (statusCode, errors) = GetHttpStatusCodeAndError(ex);

        context.Response.StatusCode = (int)statusCode;

        var response = new ApiResponse { Errors = errors, StatusCode = statusCode };

        await context.Response.WriteAsJsonAsync(response);
    }

    private static (
        HttpStatusCode statusCode,
        IReadOnlyCollection<Error> errors
    ) GetHttpStatusCodeAndError(Exception exception) =>
        exception switch
        {
            ValidationException validationException
                => (HttpStatusCode.BadRequest, validationException.Errors),
            DomainException domainException
                => (HttpStatusCode.BadRequest, new[] { domainException.Error }),
            _ => (HttpStatusCode.InternalServerError, new[] { DomainErrors.General.ServerError })
        };
}
