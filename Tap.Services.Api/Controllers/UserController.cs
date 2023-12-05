using Microsoft.AspNetCore.Mvc;
using Tap.Services.Api.Contracts;
using Tap.Services.Api.Infrastructure;

namespace Tap.Services.Api.Controllers;

public class UserController : ApiController
{
    [HttpGet(ApiRoutes.User.Get)]
    public IActionResult Get()
    {
        return Ok("Hello World!");
    }
}
