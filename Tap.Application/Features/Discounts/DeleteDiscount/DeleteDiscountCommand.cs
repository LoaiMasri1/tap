using Tap.Application.Core.Messaging;
using Tap.Domain.Core.Primitives.Result;

namespace Tap.Application.Features.Discounts.DeleteDiscount;

public record DeleteDiscountCommand(int Id) : ICommand<Result>;
