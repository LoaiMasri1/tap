﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tap.Application.Features.Amenities.CreateAmenity;
using Tap.Application.Features.Hotels.UpdateHotel;
using Tap.Application.Features.Photos.UploadPhoto;
using Tap.Contracts.Features.Amenities;
using Tap.Contracts.Features.Hotels;
using Tap.Domain.Common.Enumerations;
using Tap.Domain.Core.Errors;
using Tap.Domain.Core.Primitives.Result;
using Tap.Domain.Features.Amenities;
using Tap.Services.Api.Contracts;
using Tap.Services.Api.Infrastructure;

namespace Tap.Services.Api.Controllers;

public class HotelController : ApiController
{
    [HttpPost(ApiRoutes.Hotel.UploadPhotos)]
    [Authorize]
    public async Task<IActionResult> UploadPhotos(int id, [FromForm] IFormCollection files) =>
        await Result
            .Create((id, files))
            .Map(x => new UploadPhotosCommand(x.id, ItemType.Hotel, x.files.CreateFileRequest()))
            .Bind(x => Mediator.Send(x))
            .Match(Ok, BadRequest);

    [HttpPut(ApiRoutes.Hotel.Update)]
    [Authorize]
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
    [Authorize]
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
}
