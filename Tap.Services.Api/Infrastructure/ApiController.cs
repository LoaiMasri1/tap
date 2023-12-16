﻿using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Tap.Domain.Core.Primitives;
using Tap.Services.Api.Contracts;

namespace Tap.Services.Api.Infrastructure;

[ApiController]
[Produces("application/json")]
[Route("api/v{version:ApiVersion}")]
[ApiVersion("1.0")]
public class ApiController : ControllerBase
{
    private IMediator? _mediator;
    protected IMediator Mediator =>
        _mediator ??= HttpContext.RequestServices.GetService<IMediator>()!;

    protected IActionResult BadRequest(Error error) =>
        BadRequest(
            new ApiResponse { Errors = new[] { error }, StatusCode = HttpStatusCode.BadRequest }
        );

    protected new IActionResult Ok(object value, string? message) =>
        base.Ok(
            new ApiResponse
            {
                Data = value,
                Message = message,
                StatusCode = HttpStatusCode.OK
            }
        );

    protected new IActionResult NotFound() => base.NotFound();
}
