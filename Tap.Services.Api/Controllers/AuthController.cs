using Microsoft.AspNetCore.Mvc;
using Tap.Application.Features.Authentication.LoginUser;
using Tap.Contracts.Features.Authentication;
using Tap.Domain.Core.Primitives.Result;
using Tap.Services.Api.Contracts;
using Tap.Services.Api.Infrastructure;

namespace Tap.Services.Api.Controllers;

public class AuthController : ApiController
{
    [HttpPost(ApiRoutes.Auth.Login)]
    public async Task<IActionResult> Login(LoginRequest loginRequest) =>
        await Result
            .Create(loginRequest)
            .Map(request => new LoginUserCommand(request.Email, request.Password))
            .Bind(command => Mediator.Send(command))
            .Match(Ok, BadRequest);
}
