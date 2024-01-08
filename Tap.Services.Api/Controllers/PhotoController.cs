using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tap.Application.Features.Photos.DeletePhoto;
using Tap.Application.Features.Photos.GetPhotos;
using Tap.Application.Features.Photos.UpdatePhoto;
using Tap.Application.Features.Photos.UploadPhoto;
using Tap.Domain.Common.Enumerations;
using Tap.Domain.Core.Errors;
using Tap.Domain.Core.Primitives.Maybe;
using Tap.Domain.Core.Primitives.Result;
using Tap.Services.Api.Contracts;
using Tap.Services.Api.Infrastructure;

namespace Tap.Services.Api.Controllers;

/// <summary>
/// This class represents the PhotoController which handles the upload, update, and delete operations for photos.
/// </summary>
[Authorize]
public class PhotoController : ApiController
{
    /// <summary>
    /// Uploads a photo.
    /// </summary>
    /// <param name="id">The ID of the photo.</param>
    /// <param name="type">The type of the photo.</param>
    /// <param name="files">The collection of files to upload.</param>
    /// <response code="200">The photo was uploaded successfully.</response>
    /// <response code="400">The photo was not uploaded successfully.</response>
    /// <returns>The result of the upload operation.</returns>
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

    /// <summary>
    /// Updates a photo.
    /// </summary>
    /// <param name="id">The ID of the photo.</param>
    /// <param name="file">The file to update.</param>
    /// <response code="200">The photo was updated successfully.</response>
    /// <response code="400">The photo was not updated successfully.</response>
    /// <returns>The result of the update operation.</returns>
    [HttpPut(ApiRoutes.Photo.Update)]
    public async Task<IActionResult> Update(int id, [FromForm] IFormFile file) =>
        await Result
            .Create((id, file))
            .Ensure(x => x.id == id, DomainErrors.General.UnProcessableRequest)
            .Map(x => new UpdatePhotoCommand(x.id, x.file.CreateFileRequest()))
            .Bind(x => Mediator.Send(x))
            .Match(Ok, BadRequest);

    /// <summary>
    /// Deletes a photo.
    /// </summary>
    /// <param name="id">The ID of the photo.</param>
    /// <response code="200">The photo was deleted successfully.</response>
    /// <response code="400">The photo was not deleted successfully.</response>
    /// <returns>The result of the delete operation.</returns>

    [HttpDelete(ApiRoutes.Photo.Delete)]
    public async Task<IActionResult> Delete(int id) =>
        await Result
            .Create(id)
            .Map(x => new DeletePhotoCommand(x))
            .Bind(x => Mediator.Send(x))
            .Match(Ok, BadRequest);

    /// <summary>
    /// Retrieves photos based on specified filters, page number, and page size.
    /// </summary>
    /// <param name="filters">The filters to apply to the photos.</param>
    /// <param name="page">The page number.</param>
    /// <param name="pageSize">The number of photos per page.</param>
    /// <response code="200">The photos were retrieved successfully.</response>
    /// <response code="400">The photos were not retrieved successfully.</response>
    /// <returns>The result of the retrieval operation.</returns>
    [HttpGet(ApiRoutes.Photo.Get)]
    [AllowAnonymous]
    public async Task<IActionResult> Get(string filters, int page = 1, int pageSize = 10) =>
        await Maybe<GetPhotosQuery>
            .From(new GetPhotosQuery(page, pageSize, filters))
            .Bind(x => Mediator.Send(x))
            .Match(Ok, BadRequest);
}
