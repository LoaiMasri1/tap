using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tap.Application.Features.Reviews.CreateReview;
using Tap.Application.Features.Reviews.DeleteReview;
using Tap.Contracts.Features.Reviews;
using Tap.Domain.Core.Errors;
using Tap.Domain.Core.Primitives.Result;
using Tap.Services.Api.Contracts;
using Tap.Services.Api.Infrastructure;

namespace Tap.Services.Api.Controllers;

[Authorize]
public class ReviewController : ApiController
{
    [HttpPost(ApiRoutes.Review.Post)]
    public async Task<IActionResult> CreateReview(
        [FromQuery] [Required] int hotelId,
        [FromQuery] [Required] int userId,
        [FromBody] CreateReviewRequest request
    ) =>
        await Result
            .Create((hotelId, userId, request))
            .Ensure(x => x.hotelId == request.HotelId, DomainErrors.General.UnProcessableRequest)
            .Ensure(x => x.userId == request.UserId, DomainErrors.General.UnProcessableRequest)
            .Map(
                x =>
                    new CreateReviewCommand(
                        x.request.Title,
                        x.request.Content,
                        x.request.Rating,
                        x.hotelId,
                        x.userId
                    )
            )
            .Bind(x => Mediator.Send(x))
            .Match(Ok, BadRequest);

    [HttpDelete(ApiRoutes.Review.Delete)]
    public async Task<IActionResult> DeleteReview(int id) =>
        await Mediator.Send(new DeleteReviewCommand(id)).Match(Ok, BadRequest);
}
