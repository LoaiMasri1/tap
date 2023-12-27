using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tap.Application.Features.Photos.DeletePhoto;
using Tap.Application.Features.Photos.UpdatePhoto;
using Tap.Application.Features.Photos.UploadPhoto;
using Tap.Domain.Common.Enumerations;
using Tap.Domain.Core.Errors;
using Tap.Domain.Core.Primitives.Result;
using Tap.Services.Api.Contracts;
using Tap.Services.Api.Infrastructure;

namespace Tap.Services.Api.Controllers;

[Authorize]
public class PhotoController : ApiController
{
    [HttpPost(ApiRoutes.Photo.Upload)]
    public async Task<IActionResult> Upload(
        int id,
        ItemType type,
        [FromForm] IFormCollection files
    ) =>
        await Result
            .Create(files)
            .Map(x => new UploadPhotosCommand(id, type, x.CreateFileRequest()))
            .Bind(x => Mediator.Send(x))
            .Match(Ok, BadRequest);

    [HttpPut(ApiRoutes.Photo.Update)]
    public async Task<IActionResult> Update(int id, [FromForm] IFormFile file) =>
        await Result
            .Create((id, file))
            .Ensure(x => x.id == id, DomainErrors.General.UnProcessableRequest)
            .Map(x => new UpdatePhotoCommand(x.id, x.file.CreateFileRequest()))
            .Bind(x => Mediator.Send(x))
            .Match(Ok, BadRequest);

    [HttpDelete(ApiRoutes.Photo.Delete)]
    public async Task<IActionResult> Delete(int id) =>
        await Result
            .Create(id)
            .Map(x => new DeletePhotoCommand(x))
            .Bind(x => Mediator.Send(x))
            .Match(Ok, BadRequest);
}
