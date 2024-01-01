﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tap.Application.Features.Amenities.CreateAmenity;
using Tap.Application.Features.Hotels.GetHotelById;
using Tap.Application.Features.Hotels.SearchHotel;
using Tap.Application.Features.Hotels.UpdateHotel;
using Tap.Application.Features.Photos.UploadPhoto;
using Tap.Application.Features.Rooms.CreateRoom;
using Tap.Contracts.Features.Amenities;
using Tap.Contracts.Features.Hotels;
using Tap.Contracts.Features.Rooms;
using Tap.Domain.Common.Enumerations;
using Tap.Domain.Core.Errors;
using Tap.Domain.Core.Primitives.Maybe;
using Tap.Domain.Core.Primitives.Result;
using Tap.Domain.Features.Amenities;
using Tap.Services.Api.Contracts;
using Tap.Services.Api.Infrastructure;

namespace Tap.Services.Api.Controllers;

[Authorize]
public class HotelController : ApiController
{
    [HttpPost(ApiRoutes.Hotel.UploadPhotos)]
    public async Task<IActionResult> UploadPhotos(int id, [FromForm] IFormCollection files) =>
        await Result
            .Create((id, files))
            .Map(x => new UploadPhotosCommand(x.id, ItemType.Hotel, x.files.CreateFileRequest()))
            .Bind(x => Mediator.Send(x))
            .Match(Ok, BadRequest);

    [HttpPut(ApiRoutes.Hotel.Update)]
    public async Task<IActionResult> Update(int id, UpdateHotelRequest request) =>
        await Result
            .Create((id, request))
            .Ensure(x => x.id == x.request.Id, DomainErrors.General.UnProcessableRequest)
            .Map(
                x =>
                    new UpdateHotelCommand(
                        x.id,
                        x.request.Name,
                        x.request.Description,
                        x.request.Latitude,
                        x.request.Longitude
                    )
            )
            .Bind(x => Mediator.Send(x))
            .Match(Ok, BadRequest);

    [HttpPost(ApiRoutes.Hotel.CreateAmenities)]
    public async Task<IActionResult> CreateAmenities(int id, CreateAmenityRequest request) =>
        await Result
            .Create((id, request))
            .Ensure(x => x.id == x.request.TypeId, DomainErrors.General.UnProcessableRequest)
            .Map(
                x =>
                    new CreateAmenityCommand(
                        x.request.Name,
                        x.request.Description,
                        AmenityType.Hotel,
                        x.request.TypeId
                    )
            )
            .Bind(x => Mediator.Send(x))
            .Match(Ok, BadRequest);

    [HttpPost(ApiRoutes.Hotel.CreateRoom)]
    [Authorize]
    public async Task<IActionResult> CreateRoom(int id, CreateRoomRequest request) =>
        await Result
            .Create((id, request))
            .Ensure(x => x.id == x.request.HotelId, DomainErrors.General.UnProcessableRequest)
            .Map(
                x =>
                    new CreateRoomCommand(
                        x.request.HotelId,
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

    [HttpGet(ApiRoutes.Hotel.Get)]
    [AllowAnonymous]
    public async Task<IActionResult> Get(
        string city,
        int? rating,
        int? numberOfAvailableRooms,
        string sortBy = "name",
        string sortOrder = "asc",
        int pageNumber = 1,
        int pageSize = 10
    ) =>
        await Maybe<SearchHotelsQuery>
            .From(
                new SearchHotelsQuery(
                    city,
                    rating,
                    numberOfAvailableRooms,
                    pageSize,
                    pageNumber,
                    sortBy,
                    sortOrder
                )
            )
            .Bind(x => Mediator.Send(x))
            .Match(Ok, BadRequest);

    [HttpGet(ApiRoutes.Hotel.GetById)]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(int id, bool includeRooms = false) =>
        await Maybe<GetHotelByIdQuery>
            .From(new GetHotelByIdQuery(id, includeRooms))
            .Bind(x => Mediator.Send(x))
            .Match(Ok, BadRequest);
}
