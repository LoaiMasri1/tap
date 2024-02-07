using Microsoft.AspNetCore.Mvc;
using Tap.Application.Features.Payments.SuccessPayment;
using Tap.Domain.Core.Primitives.Result;
using Tap.Services.Api.Contracts;
using Tap.Services.Api.Infrastructure;

namespace Tap.Services.Api.Controllers;

/// <summary>
/// Represents a controller for handling successful payments.
/// </summary>
public class PaymentController : ApiController
{
    /// <summary>
    /// Handles the success payment request.
    /// </summary>
    /// <param name="sessionId">The session ID.</param>
    /// <response code="200">The payment was successful.</response>
    /// <response code="400">The payment was not successful.</response>
    /// <returns>The action result.</returns>
    [HttpGet(ApiRoutes.Payment.Success)]
    public async Task<IActionResult> Success(string sessionId) =>
        await Result
            .Create(sessionId)
            .Map(x => new SuccessPaymentCommand(x))
            .Bind(x => Mediator.Send(x))
            .Match(Ok, BadRequest);
}
