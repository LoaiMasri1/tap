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
