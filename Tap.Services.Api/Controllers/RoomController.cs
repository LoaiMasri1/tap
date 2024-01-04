using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tap.Application.Features.Amenities.CreateAmenity;
using Tap.Application.Features.Discounts.CreateDiscount;
using Tap.Application.Features.Photos.UploadPhoto;
using Tap.Application.Features.Rooms.DeleteRoom;
using Tap.Application.Features.Rooms.UpdateRoom;
using Tap.Contracts.Features.Amenities;
using Tap.Contracts.Features.Discounts;
using Tap.Contracts.Features.Rooms;
using Tap.Domain.Common.Enumerations;
using Tap.Domain.Core.Errors;
using Tap.Domain.Core.Primitives.Result;
using Tap.Domain.Features.Amenities;
using Tap.Services.Api.Contracts;
using Tap.Services.Api.Infrastructure;

namespace Tap.Services.Api.Controllers;

/// <summary>
/// Controller for managing rooms.
/// </summary>
[Authorize]
public class RoomController : ApiController
{
    /// <summary>
    /// Uploads photos for a room.
    /// </summary>
    /// <param name="id">The ID of the room.</param>
    /// <param name="files">The collection of files to upload.</param>
    /// <response code="200">The photos were uploaded successfully.</response>
    /// <response code="400">The photos were not uploaded successfully.</response>
    /// <returns>The result of the upload operation.</returns>
    [HttpPost(ApiRoutes.Room.UploadPhotos)]
    public async Task<IActionResult> UploadPhotos(int id, [FromForm] IFormCollection files) =>
        await Result
            .Create((id, files))
            .Map(x => new UploadPhotosCommand(x.id, ItemType.Room, x.files.CreateFileRequest()))
            .Bind(x => Mediator.Send(x))
            .Match(Ok, BadRequest);

    /// <summary>
    /// Adds amenities to a room.
    /// </summary>
    /// <param name="id">The ID of the room.</param>
    /// <param name="command">The command containing the amenity details.</param>
    /// <response code="200">The amenities were added successfully.</response>
    /// <response code="400">The amenities were not added successfully.</response>
    /// <returns>The result of the add amenities operation.</returns>
    [HttpPost(ApiRoutes.Room.AddAmenities)]
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

    /// <summary>
    /// Adds a discount to a room.
    /// </summary>
    /// <param name="id">The ID of the room.</param>
    /// <param name="command">The command containing the discount details.</param>
    /// <response code="200">The discount was added successfully.</response>
    /// <response code="400">The discount was not added successfully.</response>
    /// <returns>The result of the add discount operation.</returns>
    [HttpPost(ApiRoutes.Room.AddDiscount)]
    public async Task<IActionResult> AddDiscount(int id, CreateDiscountRequest command) =>
        await Result
            .Create((id, command))
            .Ensure(x => x.command.RoomId == x.id, DomainErrors.General.UnProcessableRequest)
            .Map(
                x =>
                    new CreateDiscountCommand(
                        x.command.RoomId,
                        x.command.Name,
                        x.command.Description,
                        x.command.DiscountPercentage,
                        x.command.StartDate,
                        x.command.EndDate
                    )
            )
            .Bind(x => Mediator.Send(x))
            .Match(Ok, BadRequest);

    /// <summary>
    /// Updates a room.
    /// </summary>
    /// <param name="id">The ID of the room.</param>
    /// <param name="request">The request containing the updated room details.</param>
    /// <response code="200">The room was updated successfully.</response>
    /// <response code="400">The room was not updated successfully.</response>
    /// <returns>The result of the update operation.</returns>
    [HttpPut(ApiRoutes.Room.Update)]
    public async Task<IActionResult> Update(int id, UpdateRoomRequest request) =>
        await Result
            .Create((id, request))
            .Ensure(x => x.id == x.request.Id, DomainErrors.General.UnProcessableRequest)
            .Map(
                x =>
                    new UpdateRoomCommand(
                        x.id,
                        x.request.Number,
                        x.request.Price,
                        x.request.Currency,
                        x.request.Type,
                        x.request.CapacityOfAdults,
                        x.request.CapacityOfChildren
                    )
            )
            .Bind(x => Mediator.Send(x))
            .Match(Ok, BadRequest);

    /// <summary>
    /// Deletes a room.
    /// </summary>
    /// <param name="id">The ID of the room to delete.</param>
    /// <response code="200">The room was deleted successfully.</response>
    /// <response code="400">The room was not deleted successfully.</response>
    /// <returns>The result of the delete operation.</returns>
    [HttpDelete(ApiRoutes.Room.Delete)]
    public async Task<IActionResult> Delete(int id) =>
        await Result
            .Create(id)
            .Map(x => new DeleteRoomCommand(x))
            .Bind(x => Mediator.Send(x))
            .Match(Ok, BadRequest);
}
