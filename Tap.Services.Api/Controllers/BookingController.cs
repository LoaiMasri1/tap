using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tap.Application.Features.Bookings.CancelBooking;
using Tap.Application.Features.Bookings.CheckoutBooking;
using Tap.Application.Features.Bookings.ConfirmBooking;
using Tap.Application.Features.Bookings.GetBookings;
using Tap.Domain.Core.Primitives.Maybe;
using Tap.Domain.Core.Primitives.Result;
using Tap.Services.Api.Contracts;
using Tap.Services.Api.Infrastructure;

namespace Tap.Services.Api.Controllers;

/// <summary>
/// Represents a controller for managing bookings.
/// </summary>
[Authorize]
public class BookingController : ApiController
{
    /// <summary>
    /// Confirms a booking.
    /// </summary>
    /// <param name="id">The ID of the booking.</param>
    /// <response code="200">The booking was confirmed successfully.</response>
    /// <response code="400">The booking was not confirmed successfully.</response>
    /// <returns>The result of the confirmation.</returns>
    [HttpPost(ApiRoutes.Booking.Confirm)]
    public async Task<IActionResult> ConfirmBooking(int id) =>
        await Result
            .Create(id)
            .Map(x => new ConfirmBookingCommand(x))
            .Bind(x => Mediator.Send(x))
            .Match(Ok, BadRequest);

    /// <summary>
    /// Cancels a booking.
    /// </summary>
    /// <param name="id">The ID of the booking.</param>
    /// <response code="200">The booking was cancelled successfully.</response>
    /// <response code="400">The booking was not cancelled successfully.</response>
    /// <returns>The result of the cancellation.</returns>
    [HttpPost(ApiRoutes.Booking.Cancel)]
    public async Task<IActionResult> CancelBooking(int id) =>
        await Result
            .Create(id)
            .Map(x => new CancelBookingCommand(x))
            .Bind(x => Mediator.Send(x))
            .Match(Ok, BadRequest);

    /// <summary>
    /// Checks out a booking.
    /// </summary>
    /// <param name="id">The ID of the booking.</param>
    /// <response code="200">The booking was checked out successfully.</response>
    /// <response code="400">The booking was not checked out successfully.</response>
    /// <returns>The result of the checkout.</returns>
    [HttpPost(ApiRoutes.Booking.Checkout)]
    public async Task<IActionResult> CheckoutBooking(int id) =>
        await Result
            .Create(id)
            .Map(x => new CheckoutBookingCommand(x))
            .Bind(x => Mediator.Send(x))
            .Match(Ok, BadRequest);

    /// <summary>
    /// Retrieves bookings based on specified filters.
    /// </summary>
    /// <param name="filters">The filters.</param>
    /// <param name="sorts">The sorts.</param>
    /// <param name="page">The page.</param>
    /// <param name="pageSize">The page size.</param>
    /// <returns>The result of the retrieval.</returns>
    [HttpGet(ApiRoutes.Booking.Get)]
    public async Task<IActionResult> GetBookings(
        string filters,
        string sorts,
        int page = 1,
        int pageSize = 10
    ) =>
        await Maybe<GetBookingsQuery>
            .From(new GetBookingsQuery(filters, sorts, page, pageSize))
            .Bind(x => Mediator.Send(x))
            .Match(Ok, BadRequest);
}
