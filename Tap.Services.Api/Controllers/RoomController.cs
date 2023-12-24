using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tap.Application.Features.Amenities.CreateAmenity;
using Tap.Application.Features.Photos.UploadPhoto;
using Tap.Contracts.Features.Amenities;
using Tap.Domain.Common.Enumerations;
using Tap.Domain.Core.Errors;
using Tap.Domain.Core.Primitives.Result;
using Tap.Domain.Features.Amenities;
using Tap.Services.Api.Contracts;
using Tap.Services.Api.Infrastructure;

namespace Tap.Services.Api.Controllers;

public class RoomController : ApiController
{
    [HttpPost(ApiRoutes.Room.UploadPhotos)]
    [Authorize]
    public async Task<IActionResult> UploadPhotos(int id, [FromForm] IFormCollection files) =>
        await Result
            .Create((id, files))
            .Map(x => new UploadPhotosCommand(x.id, ItemType.Room, x.files.CreateFileRequest()))
            .Bind(x => Mediator.Send(x))
            .Match(Ok, BadRequest);

    [HttpPost(ApiRoutes.Room.AddAmenities)]
    [Authorize]
    public async Task<IActionResult> AddAmenities(int id, CreateAmenityRequest command) =>
        await Result
            .Create((id, command))
            .Ensure(x => x.command.TypeId == x.id, DomainErrors.General.UnProcessableRequest)
            .Map(
                x =>
                    new CreateAmenityCommand(
                        x.command.Name,
                        x.command.Description,
                        AmenityType.Room,
                        x.command.TypeId
                    )
            )
            .Bind(x => Mediator.Send(x))
            .Match(Ok, BadRequest);
}
