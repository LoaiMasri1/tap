﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tap.Application.Features.Amenities.DeleteAmenity;
using Tap.Application.Features.Amenities.UpdateAmenity;
using Tap.Contracts.Features.Amenities;
using Tap.Domain.Core.Errors;
using Tap.Domain.Core.Primitives.Result;
using Tap.Services.Api.Contracts;
using Tap.Services.Api.Infrastructure;

namespace Tap.Services.Api.Controllers;

/// <summary>
/// Controller for managing amenities.
/// </summary>
public class AmenityController : ApiController
{
    /// <summary>
    /// Updates an amenity.
    /// </summary>
    /// <param name="id">The ID of the amenity.</param>
    /// <param name="request">The update request.</param>
    /// <response code="200">The amenity was updated successfully.</response>
    /// <response code="400">The amenity was not updated successfully.</response>
    /// <returns>The updated amenity.</returns>
    [HttpPut(ApiRoutes.Amenity.Update)]
    [Authorize]
    public async Task<IActionResult> Update(int id, UpdateAmenityRequest request) =>
        await Result
            .Create((id, request))
            .Ensure(x => x.id == x.request.Id, DomainErrors.General.UnProcessableRequest)
            .Map(x => new UpdateAmenityCommand(x.id, x.request.Name, x.request.Description))
            .Bind(x => Mediator.Send(x))
            .Match(Ok, BadRequest);

    /// <summary>
    /// Deletes an amenity.
    /// </summary>
    /// <param name="id">The ID of the amenity to delete.</param>
    /// <response code="200">The amenity was deleted successfully.</response>
    /// <response code="400">The amenity was not deleted successfully.</response>
    /// <returns>The result of the deletion.</returns>
    [HttpDelete(ApiRoutes.Amenity.Delete)]
    [Authorize]
    public async Task<IActionResult> Delete(int id) =>
        await Result
            .Create(id)
            .Map(x => new DeleteAmenityCommand(x))
            .Bind(x => Mediator.Send(x))
            .Match(Ok, BadRequest);
}
