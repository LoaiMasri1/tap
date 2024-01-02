using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tap.Application.Features.Cities.CreateCity;
using Tap.Application.Features.Cities.GetCities;
using Tap.Application.Features.Hotels.CreateHotel;
using Tap.Application.Features.Photos.UploadPhoto;
using Tap.Contracts.Features.Cities;
using Tap.Contracts.Features.Hotels;
using Tap.Domain.Common.Enumerations;
using Tap.Domain.Core.Primitives.Maybe;
using Tap.Domain.Core.Primitives.Result;
using Tap.Services.Api.Contracts;
using Tap.Services.Api.Infrastructure;

namespace Tap.Services.Api.Controllers;

[Authorize]
public class CityController : ApiController
{
    [HttpPost(ApiRoutes.City.Post)]
    public async Task<IActionResult> Create(CreateCityRequest request) =>
        await Result
            .Create(request)
            .Map(x => new CreateCityCommand(x.Name, x.Description, x.Country))
            .Bind(x => Mediator.Send(x))
            .Match(Ok, BadRequest);

    [HttpPost(ApiRoutes.City.CreateHotel)]
    public async Task<IActionResult> Create(int id, CreateHotelRequest request) =>
        await Result
            .Create(request)
            .Map(x => new CreateHotelCommand(x.Name, x.Description, x.Latitude, x.Longitude, id))
            .Bind(x => Mediator.Send(x))
            .Match(Ok, BadRequest);

    [HttpPost(ApiRoutes.City.UploadPhotos)]
    public async Task<IActionResult> UploadPhotos(int id, [FromForm] IFormCollection files) =>
        await Result
            .Create((id, files))
            .Map(x => new UploadPhotosCommand(x.id, ItemType.City, x.files.CreateFileRequest()))
            .Bind(x => Mediator.Send(x))
            .Match(Ok, BadRequest);

    [HttpGet(ApiRoutes.City.Get)]
    [AllowAnonymous]
    public async Task<IActionResult> Get(
        string sortBy = "name",
        string sortOrder = "asc",
        int pageNumber = 1,
        int pageSize = 10
    ) =>
        await Maybe<GetCitiesQuery>
            .From(new GetCitiesQuery(pageSize, pageNumber, sortBy, sortOrder))
            .Bind(x => Mediator.Send(x))
            .Match(Ok, BadRequest);
}
