using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Tap.Application.Features.Authentication.ActivateUser;
using Tap.Application.Features.Authentication.LoginUser;
using Tap.Application.Features.Authentication.RegisterUser;
using Tap.Contracts.Features.Authentication;
using Tap.Contracts.Features.Users;
using Tap.Domain.Core.Primitives.Result;
using Tap.Services.Api.Contracts;
using Tap.Services.Api.Infrastructure;

namespace Tap.Services.Api.Controllers;

/// <summary>
/// Controller for handling authentication related operations.
/// </summary>
public class AuthController : ApiController
{
    /// <summary>
    /// Logs in a user.
    /// </summary>
    /// <param name="loginRequest">The login request.</param>
    /// <response code="200">The user was logged in successfully.</response>
    /// <response code="400">The user was not logged in successfully.</response>
    /// <returns>The login result.</returns>
    [HttpPost(ApiRoutes.Auth.Login)]
    public async Task<IActionResult> Login(LoginRequest loginRequest) =>
        await Result
            .Create(loginRequest)
            .Map(request => new LoginUserCommand(request.Email, request.Password))
            .Bind(command => Mediator.Send(command))
            .Match(Ok, BadRequest);

    /// <summary>
    /// Creates a new user.
    /// </summary>
    /// <param name="createUserRequest">The create user request.</param>
    /// <response code="200">The user was created successfully.</response>
    /// <response code="400">The user was not created successfully.</response>
    /// <returns>The creation user result.</returns>
    [HttpPost(ApiRoutes.Auth.Register)]
    public async Task<IActionResult> Create(CreateUserRequest createUserRequest) =>
        await Result
            .Create(createUserRequest)
            .Map(
                request =>
                    new RegisterUserCommand(
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

    /// <summary>
    /// Activates a user account.
    /// </summary>
    /// <param name="token">The activation token.</param>
    /// <response code="200">The user was activated successfully.</response>
    /// <response code="400">The user was not activated successfully.</response>
    /// <returns>The activation result.</returns>
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
