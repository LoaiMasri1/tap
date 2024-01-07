using Tap.Application.Core.Abstractions.Common;
using Tap.Application.Core.Abstractions.Data;
using Tap.Application.Core.Messaging;
using Tap.Application.Features.Authentication;
using Tap.Contracts.Features.Discounts;
using Tap.Domain.Core.Errors;
using Tap.Domain.Core.Primitives.Result;
using Tap.Domain.Features.Discounts;
using Tap.Domain.Features.Rooms;
using Tap.Domain.Features.Users;

namespace Tap.Application.Features.Discounts.CreateDiscount;

public class CreateDiscountCommandHandler
    : ICommandHandler<CreateDiscountCommand, Result<DiscountResponse>>
{
    private readonly IRoomRepository _roomRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTime _dateTime;
    private readonly IUserContext _userContext;

    public CreateDiscountCommandHandler(
        IRoomRepository roomRepository,
        IUnitOfWork unitOfWork,
        IUserContext userContext,
        IDateTime dateTime
    )
    {
        _roomRepository = roomRepository;
        _unitOfWork = unitOfWork;
        _userContext = userContext;
        _dateTime = dateTime;
    }

    public async Task<Result<DiscountResponse>> Handle(
        CreateDiscountCommand command,
        CancellationToken cancellationToken
    )
    {
        Console.WriteLine($"User id: {_userContext.Id}, User role: {_userContext.Role}");
        var userRole = _userContext.Role;
        if (userRole != UserRole.Admin)
        {
            return DomainErrors.User.Unauthorized;
        }

        var maybeRoom = await _roomRepository.GetByIdWithDiscountsAsync(
            command.RoomId,
            cancellationToken
        );

        if (maybeRoom.HasNoValue)
        {
            return DomainErrors.Room.NotFound;
        }

        var room = maybeRoom.Value;

        Console.WriteLine($"Discounts count before adding: {room.Discounts.Count}");

        var discount = Discount.Create(
            command.Name,
            command.Description,
            command.DiscountPercentage,
            command.StartDate,
            command.EndDate
        );

        var result = room.AddDiscount(discount, _dateTime.UtcNow);
        if (result.IsFailure)
        {
            return result.Error;
        }

        Console.WriteLine($"Discounts count after adding: {room.Discounts.Count}");

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new DiscountResponse(
            discount.Id,
            discount.DiscountPercentage,
            discount.StartDate,
            discount.EndDate
        );
    }
}
