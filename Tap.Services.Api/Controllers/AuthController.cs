using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Tap.Application.Features.Authentication.LoginUser;
using Tap.Application.Features.Users.ActivateUser;
using Tap.Application.Features.Users.CreateUser;
using Tap.Contracts.Features.Authentication;
using Tap.Contracts.Features.Users;
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

    [HttpPost(ApiRoutes.Auth.Register)]
    public async Task<IActionResult> Create(CreateUserRequest createUserRequest) =>
        await Result
            .Create(createUserRequest)
            .Map(
                request =>
                    new CreateUserCommand(
                        $"{request.FirstName} {request.LastName}",
                        request.Email,
                        request.Password,
                        request.Role
                    )
            )
            .Bind(command => Mediator.Send(command))
            .Match(
                user =>
                    Ok(
                        user,
                        "Account Created Successfully. Please check your email for activation."
                    ),
                BadRequest
            );

    [HttpGet(ApiRoutes.Auth.Activate)]
    public async Task<IActionResult> Activate([FromQuery(Name = "t")] [Required] string token) =>
        await Result
            .Create(token)
            .Map(t => new ActivateUserCommand(t))
            .Bind(command => Mediator.Send(command))
            .Match(
                user => Ok(user, "Account Activated Successfully. You can now login."),
                BadRequest
            );
}
