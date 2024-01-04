using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tap.Application.Features.Users.UpdateUser;
using Tap.Contracts.Features.Users;
using Tap.Domain.Core.Errors;
using Tap.Domain.Core.Primitives.Result;
using Tap.Services.Api.Contracts;
using Tap.Services.Api.Infrastructure;

namespace Tap.Services.Api.Controllers;

[Authorize]
public class UserController : ApiController
{
    /// <summary>
    /// Updates a user.
    /// </summary>
    /// <param name="id">The ID of the user.</param>
    /// <param name="updateUserRequest">The request object containing the updated user information.</param>
    /// <response code="200">The user was updated successfully.</response>
    /// <response code="400">The user was not updated successfully.</response>
    /// <returns>The result of the update operation.</returns>
    [HttpPut(ApiRoutes.User.Update)]
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
