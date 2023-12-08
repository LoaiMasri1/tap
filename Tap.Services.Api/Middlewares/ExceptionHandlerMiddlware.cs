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
    private readonly ILogger<ExceptionHandlerMiddlware> _logger;

    public ExceptionHandlerMiddlware(ILogger<ExceptionHandlerMiddlware> logger) => _logger = logger;

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
        (HttpStatusCode statusCode, IReadOnlyCollection<Error> errors) = GetHttpStatusCodeAndError(
            ex
        );

        context.Response.StatusCode = (int)statusCode;

        string response = JsonSerializer.Serialize(
            new ApiResponse { Errors = errors, StatusCode = statusCode }
        );

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
