using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tap.Application.Public.GetFeaturedDeals;
using Tap.Application.Public.GetTrendingDestination;
using Tap.Domain.Core.Primitives.Maybe;
using Tap.Services.Api.Contracts;
using Tap.Services.Api.Infrastructure;

namespace Tap.Services.Api.Controllers;

/// <summary>
/// Represents a controller for handling public API requests.
/// </summary>
[AllowAnonymous]
public class PublicController : ApiController
{
    /// <summary>
    /// Retrieves the trending destinations.
    /// </summary>
    /// <param name="limit">The maximum number of destinations to retrieve.</param>
    /// <response code="200">The trending destinations were retrieved successfully.</response>
    /// <response code="400">The trending destinations were not retrieved successfully.</response>
    /// <returns>An IActionResult representing the result of the operation.</returns>
    [HttpGet(ApiRoutes.Public.TrendingDestinations)]
    public async Task<IActionResult> GetTrendingDestinations(int limit = 5) =>
        await Maybe<GetTrendingDestinationQuery>
            .From(new GetTrendingDestinationQuery(limit))
            .Bind(x => Mediator.Send(x))
            .Match(Ok, BadRequest);

    /// <summary>
    /// Retrieves the featured deals.
    /// </summary>
    /// <param name="limit">The maximum number of deals to retrieve.</param>
    /// <response code="200">The featured deals were retrieved successfully.</response>
    /// <response code="400">The featured deals were not retrieved successfully.</response>
    /// <returns>An IActionResult representing the result of the operation.</returns>
    [HttpGet(ApiRoutes.Public.FeaturedDeals)]
    public async Task<IActionResult> GetFeaturedDeals(int limit = 5) =>
        await Maybe<GetFeaturedDealsQuery>
            .From(new GetFeaturedDealsQuery(limit))
            .Bind(x => Mediator.Send(x))
            .Match(Ok, BadRequest);
}
