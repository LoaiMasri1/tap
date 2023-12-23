using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tap.Application.Features.Users.ActivateUser;
using Tap.Application.Features.Users.CreateUser;
using Tap.Application.Features.Users.UpdateUser;
using Tap.Contracts.Features.Users;
using Tap.Domain.Core.Errors;
using Tap.Domain.Core.Primitives.Result;
using Tap.Services.Api.Contracts;
using Tap.Services.Api.Infrastructure;

namespace Tap.Services.Api.Controllers;

public class UserController : ApiController
{
    [HttpPost(ApiRoutes.User.Post)]
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

    [HttpGet(ApiRoutes.User.Activate)]
    public async Task<IActionResult> Activate([FromQuery(Name = "t")] [Required] string token) =>
        await Result
            .Create(token)
            .Map(t => new ActivateUserCommand(t))
            .Bind(command => Mediator.Send(command))
            .Match(
                user => Ok(user, "Account Activated Successfully. You can now login."),
                BadRequest
            );

    [HttpPost(ApiRoutes.User.Update)]
    [Authorize]
    public async Task<IActionResult> Update(int id, UpdateUserRequest updateUserRequest) =>
        await Result
            .Create(updateUserRequest)
            .Ensure(request => request.Id == id, DomainErrors.General.UnProcessableRequest)
            .Map(
                request =>
                    new UpdateUserCommand(
                        request.Id,
                        request.FirstName,
                        request.LastName,
                        request.Password,
                        request.ConfirmPassword
                    )
            )
            .Bind(command => Mediator.Send(command))
            .Match(Ok, BadRequest);
}
