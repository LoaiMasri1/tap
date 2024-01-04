using Microsoft.AspNetCore.Mvc;
using Tap.Application.Features.Payments.SuccessPayment;
using Tap.Domain.Core.Primitives.Result;
using Tap.Services.Api.Contracts;
using Tap.Services.Api.Infrastructure;

namespace Tap.Services.Api.Controllers;

public class PaymentController : ApiController
{
    [HttpGet(ApiRoutes.Payment.Success)]
    public async Task<IActionResult> Success(string sessionId) =>
        await Result
            .Create(sessionId)
            .Map(x => new SuccessPaymentCommand(x))
            .Bind(x => Mediator.Send(x))
            .Match(Ok, BadRequest);
}
