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
    /// <param name="sorts">The sort criteria.</param>
    /// <param name="filters">The filter criteria.</param>
    /// <param name="page">The page number.</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <returns>The discounts for the specific hotel.</returns>
    [HttpGet(ApiRoutes.Discount.GetByHotelId)]
    [AllowAnonymous]
    public async Task<IActionResult> GetDiscount(
        int id,
        string sorts,
        string filters,
        int page = 1,
        int pageSize = 10
    ) =>
        await Maybe<GetDiscountsForSpecificHotelQuery>
            .From(new GetDiscountsForSpecificHotelQuery(page, pageSize, sorts, filters, id))
            .Bind(x => Mediator.Send(x))
            .Match(Ok, BadRequest);

    /// <summary>
    /// Deletes a discount.
    /// </summary>
    /// <param name="id">The ID of the discount to delete.</param>
    /// <returns>The result of the delete operation.</returns>
    [HttpDelete(ApiRoutes.Discount.Delete)]
    [Authorize]
    public async Task<IActionResult> DeleteDiscount(int id) =>
        await Result
            .Create(id)
            .Map(x => new DeleteDiscountCommand(x))
            .Bind(x => Mediator.Send(x))
            .Match(Ok, BadRequest);
}
