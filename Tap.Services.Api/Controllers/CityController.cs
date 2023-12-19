using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tap.Application.Features.Cities.CreateCity;
using Tap.Application.Features.Hotels.CreateHotel;
using Tap.Contracts.Features.Cities;
using Tap.Contracts.Features.Hotels;
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
}
