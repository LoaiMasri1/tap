using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tap.Application.Features.Discounts.DeleteDiscount;
using Tap.Application.Features.Discounts.GetDiscountsForSpecificHotel;
using Tap.Domain.Core.Primitives.Maybe;
using Tap.Domain.Core.Primitives.Result;
using Tap.Services.Api.Contracts;
using Tap.Services.Api.Infrastructure;

namespace Tap.Services.Api.Controllers;

public class DiscountController : ApiController
{
    /// <summary>
    /// Retrieves discounts for a specific hotel.
    /// </summary>
    /// <param name="id">The ID of the hotel.</param>
    /// <param name="sortBy">The field to sort by.</param>
    /// <param name="sortOrder">The sort order (asc or desc).</param>
    /// <param name="pageNumber">The page number.</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <returns>The discounts for the specific hotel.</returns>
    [HttpGet(ApiRoutes.Discount.GetByHotelId)]
    [AllowAnonymous]
    public async Task<IActionResult> GetDiscount(
        int id,
        string sortBy = "id",
        string sortOrder = "asc",
        int pageNumber = 1,
        int pageSize = 10
    ) =>
        await Maybe<GetDiscountsForSpecificHotelQuery>
            .From(
                new GetDiscountsForSpecificHotelQuery(pageNumber, pageSize, sortBy, sortOrder, id)
            )
            .Bind(x => Mediator.Send(x))
            .Match(Ok, BadRequest);

    /// <summary>
    /// Deletes a discount.
    /// </summary>
    /// <param name="id">The ID of the discount to delete.</param>
    /// <returns>The result of the delete operation.</returns>
    [HttpPost(ApiRoutes.Discount.Delete)]
    [Authorize]
    public async Task<IActionResult> DeleteDiscount(int id) =>
        await Result
            .Create(id)
            .Map(x => new DeleteDiscountCommand(x))
            .Bind(x => Mediator.Send(x))
            .Match(Ok, BadRequest);
}
