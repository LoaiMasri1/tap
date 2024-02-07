using Microsoft.EntityFrameworkCore;
using Tap.Application.Core.Abstractions.Data;
using Tap.Application.Core.Messaging;
using Tap.Application.Features.Authentication;
using Tap.Domain.Core.Errors;
using Tap.Domain.Core.Primitives.Result;
using Tap.Domain.Features.Discounts;
using Tap.Domain.Features.Users;

namespace Tap.Application.Features.Discounts.DeleteDiscount;

public class DeleteDiscountCommandHandler : ICommandHandler<DeleteDiscountCommand, Result>
{
    private readonly IDbContext _dbContext;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;

    public DeleteDiscountCommandHandler(
        IDbContext dbContext,
        IUnitOfWork unitOfWork,
        IUserContext userContext
    )
    {
        _dbContext = dbContext;
        _unitOfWork = unitOfWork;
        _userContext = userContext;
    }

    public async Task<Result> Handle(
        DeleteDiscountCommand request,
        CancellationToken cancellationToken
    )
    {
        if (_userContext.Role != UserRole.Admin)
        {
            return DomainErrors.User.Unauthorized;
        }

        var discount = await _dbContext
            .Set<Discount>()
            .FirstOrDefaultAsync(d => d.Id == request.Id, cancellationToken);

        if (discount is null)
        {
            return DomainErrors.Discount.NotFound;
        }

        _dbContext.Set<Discount>().Remove(discount);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
