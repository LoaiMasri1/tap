using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tap.Application.Features.Bookings.CancelBooking;
using Tap.Application.Features.Bookings.CheckoutRoom;
using Tap.Application.Features.Bookings.ConfirmBook;
using Tap.Domain.Core.Primitives.Result;
using Tap.Services.Api.Contracts;
using Tap.Services.Api.Infrastructure;

namespace Tap.Services.Api.Controllers;

[Authorize]
public class BookingController : ApiController
{
    [HttpPost(ApiRoutes.Booking.Confirm)]
    public async Task<IActionResult> ConfirmBooking(int id) =>
        await Result
            .Create(id)
            .Map(x => new ConfirmBookCommand(x))
            .Bind(x => Mediator.Send(x))
            .Match(Ok, BadRequest);

    [HttpPost(ApiRoutes.Booking.Cancel)]
    public async Task<IActionResult> CancelBooking(int id) =>
        await Result
            .Create(id)
            .Map(x => new CancelBookingCommand(x))
            .Bind(x => Mediator.Send(x))
            .Match(Ok, BadRequest);

    [HttpPost(ApiRoutes.Booking.Checkout)]
    public async Task<IActionResult> CheckoutBooking(int id) =>
        await Result
            .Create(id)
            .Map(x => new CheckoutBookingCommand(x))
            .Bind(x => Mediator.Send(x))
            .Match(Ok, BadRequest);
}
