using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tap.Application.Features.Cities.CreateCity;
using Tap.Application.Features.Cities.GetCities;
using Tap.Application.Features.Cities.GetCityById;
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

/// <summary>
/// Controller for managing cities.
/// </summary>
[Authorize]
public class CityController : ApiController
{
    /// <summary>
    /// Creates a new city.
    /// </summary>
    /// <param name="request">The request containing the city details.</param>
    /// <response code="200">The city was created successfully.</response>
    /// <response code="400">The city was not created successfully.</response>
    /// <returns>The result of the create operation.</returns>
    [HttpPost(ApiRoutes.City.Post)]
    public async Task<IActionResult> Create(CreateCityRequest request) =>
        await Result
            .Create(request)
            .Map(x => new CreateCityCommand(x.Name, x.Description, x.Country))
            .Bind(x => Mediator.Send(x))
            .Match(Ok, BadRequest);

    /// <summary>
    /// Creates a new hotel in a city.
    /// </summary>
    /// <param name="id">The ID of the city.</param>
    /// <param name="request">The request containing the hotel details.</param>
    /// <response code="200">The hotel was created successfully.</response>
    /// <response code="400">The hotel was not created successfully.</response>
    /// <returns>The result of the create operation.</returns>
    [HttpPost(ApiRoutes.City.CreateHotel)]
    public async Task<IActionResult> Create(int id, CreateHotelRequest request) =>
        await Result
            .Create(request)
            .Map(x => new CreateHotelCommand(x.Name, x.Description, x.Latitude, x.Longitude, id))
            .Bind(x => Mediator.Send(x))
            .Match(Ok, BadRequest);

    /// <summary>
    /// Uploads photos for a city.
    /// </summary>
    /// <param name="id">The ID of the city.</param>
    /// <param name="files">The collection of files to upload.</param>
    /// <response code="200">The photos were uploaded successfully.</response>
    /// <response code="400">The photos were not uploaded successfully.</response>
    /// <returns>The result of the upload operation.</returns>
    [HttpPost(ApiRoutes.City.UploadPhotos)]
    public async Task<IActionResult> UploadPhotos(int id, [FromForm] IFormCollection files) =>
        await Result
            .Create((id, files))
            .Map(x => new UploadPhotosCommand(x.id, ItemType.City, x.files.CreateFileRequest()))
            .Bind(x => Mediator.Send(x))
            .Match(Ok, BadRequest);

    /// <summary>
    /// Gets a list of cities.
    /// </summary>
    /// <param name="filters">The filters.</param>
    /// <param name ="sorts">The sorts.</param>
    /// <param name="page">The page.</param>
    /// <param name="pageSize">The page size.</param>
    /// <response code="200">The list of cities was retrieved successfully.</response>
    /// <response code="400">The list of cities was not retrieved successfully.</response>
    /// <returns>The list of cities.</returns>
    [HttpGet(ApiRoutes.City.Get)]
    [AllowAnonymous]
    public async Task<IActionResult> Get(
        string filters,
        string sorts,
        int page = 1,
        int pageSize = 10
    ) =>
        await Maybe<GetCitiesQuery>
            .From(new GetCitiesQuery(filters, sorts, page, pageSize))
            .Bind(x => Mediator.Send(x))
            .Match(Ok, BadRequest);

    /// <summary>
    /// Gets a city by ID.
    /// </summary>
    /// <param name="id">The ID of the city.</param>
    /// <response code="200">The city with the specified ID was retrieved successfully.</response>
    /// <response code="400">The city with the specified ID was not retrieved successfully.</response>
    /// <returns>The city with the specified ID.</returns>
    [HttpGet(ApiRoutes.City.GetById)]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(int id) =>
        await Maybe<GetCityByIdQuery>
            .From(new GetCityByIdQuery(id))
            .Bind(x => Mediator.Send(x))
            .Match(Ok, BadRequest);
}
