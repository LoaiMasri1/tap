using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tap.Application.Features.Amenities.CreateAmenity;
using Tap.Application.Features.Bookings.BookRoom;
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

[Authorize]
public class RoomController : ApiController
{
    [HttpPost(ApiRoutes.Room.UploadPhotos)]
    public async Task<IActionResult> UploadPhotos(int id, [FromForm] IFormCollection files) =>
        await Result
            .Create((id, files))
            .Map(x => new UploadPhotosCommand(x.id, ItemType.Room, x.files.CreateFileRequest()))
            .Bind(x => Mediator.Send(x))
            .Match(Ok, BadRequest);

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

    [HttpDelete(ApiRoutes.Room.Delete)]
    public async Task<IActionResult> Delete(int id) =>
        await Result
            .Create(id)
            .Map(x => new DeleteRoomCommand(x))
            .Bind(x => Mediator.Send(x))
            .Match(Ok, BadRequest);

    [HttpPost(ApiRoutes.Room.Book)]
    public async Task<IActionResult> Book(int id, DateTime checkInDate, DateTime checkOutDate) =>
        await Result
            .Create((id, checkInDate, checkOutDate))
            .Map(x => new BookRoomCommand(x.id, x.checkInDate, x.checkOutDate))
            .Bind(x => Mediator.Send(x))
            .Match(Ok, BadRequest);
}
