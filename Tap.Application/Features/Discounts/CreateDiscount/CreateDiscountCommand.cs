using Tap.Application.Core.Messaging;
using Tap.Contracts.Features.Discounts;
using Tap.Domain.Core.Primitives.Result;

namespace Tap.Application.Features.Discounts.CreateDiscount;

public record CreateDiscountCommand(
    int RoomId,
    string Name,
    string Description,
    decimal DiscountPercentage,
    DateTime StartDate,
    DateTime EndDate
) : ICommand<Result<DiscountResponse>>;
