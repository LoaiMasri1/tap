using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tap.Application.Features.Amenities.DeleteAmenity;
using Tap.Application.Features.Amenities.UpdateAmenity;
using Tap.Contracts.Features.Amenities;
using Tap.Domain.Core.Errors;
using Tap.Domain.Core.Primitives.Result;
using Tap.Services.Api.Contracts;
using Tap.Services.Api.Infrastructure;

namespace Tap.Services.Api.Controllers;

public class AmenityController : ApiController
{
    [HttpPut(ApiRoutes.Amenity.Update)]
    [Authorize]
    public async Task<IActionResult> Update(int id, UpdateAmenityRequest request) =>
        await Result
            .Create((id, request))
            .Ensure(x => x.id == x.request.Id, DomainErrors.General.UnProcessableRequest)
            .Map(x => new UpdateAmenityCommand(x.id, x.request.Name, x.request.Description))
            .Bind(x => Mediator.Send(x))
            .Match(Ok, BadRequest);

    [HttpDelete(ApiRoutes.Amenity.Delete)]
    [Authorize]
    public async Task<IActionResult> Delete(int id) =>
        await Result
            .Create(id)
            .Map(x => new DeleteAmenityCommand(x))
            .Bind(x => Mediator.Send(x))
            .Match(Ok, BadRequest);
}
