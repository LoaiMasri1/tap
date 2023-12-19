using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tap.Application.Features.Cities.CreateCity;
using Tap.Application.Features.Hotels.CreateHotel;
using Tap.Application.Features.Photos.UploadPhoto;
using Tap.Contracts.Features.Cities;
using Tap.Contracts.Features.Hotels;
using Tap.Contracts.Files;
using Tap.Domain.Common.Enumerations;
using Tap.Domain.Core.Primitives.Result;
using Tap.Services.Api.Contracts;
using Tap.Services.Api.Infrastructure;

namespace Tap.Services.Api.Controllers;

public class CityController : ApiController
{
    [HttpPost(ApiRoutes.City.Post)]
    [Authorize]
    public async Task<IActionResult> Create(CreateCityRequest request) =>
        await Result
            .Create(request)
            .Map(x => new CreateCityCommand(x.Name, x.Description, x.Country))
            .Bind(x => Mediator.Send(x))
            .Match(Ok, BadRequest);

    [HttpPost(ApiRoutes.City.CreateHotel)]
    [Authorize]
    public async Task<IActionResult> Create(int id, CreateHotelRequest request) =>
        await Result
            .Create(request)
            .Map(x => new CreateHotelCommand(x.Name, x.Description, x.Latitude, x.Longitude, id))
            .Bind(x => Mediator.Send(x))
            .Match(Ok, BadRequest);

    // upload photos
    [HttpPost(ApiRoutes.City.UploadPhotos)]
    [Authorize]
    public async Task<IActionResult> UploadPhotos(int id, IFormFile file)
    {
        var ms = new MemoryStream();
        await file.CopyToAsync(ms);
        var bytes = ms.ToArray();
        var filesWithBytes = new FileRequest[] { new(file.FileName, bytes) };

        return await Result
            .Create(id)
            .Map(x => new UploadPhotosCommand(ItemId: id, ItemType.City, filesWithBytes))
            .Bind(x => Mediator.Send(x))
            .Match(Ok, BadRequest);
    }
}
