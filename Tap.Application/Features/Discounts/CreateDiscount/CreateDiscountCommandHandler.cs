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
    private readonly IUserIdentifierProvider _userIdentifierProvider;

    public CreateDiscountCommandHandler(
        IRoomRepository roomRepository,
        IUnitOfWork unitOfWork,
        IUserIdentifierProvider userIdentifierProvider
    )
    {
        _roomRepository = roomRepository;
        _unitOfWork = unitOfWork;
        _userIdentifierProvider = userIdentifierProvider;
    }

    public async Task<Result<DiscountResponse>> Handle(
        CreateDiscountCommand command,
        CancellationToken cancellationToken
    )
    {
        var userRole = _userIdentifierProvider.Role;
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

        var discount = Discount.Create(
            command.Name,
            command.Description,
            command.DiscountPercentage,
            command.StartDate,
            command.EndDate
        );

        foreach (var d in room.Discounts)
        {
            Console.WriteLine(d.DiscountPercentage);
        }

        Console.WriteLine($"Discounted price before adding discount: {room.DiscountedPrice}");

        room.AddDiscount(discount);
        room.UpdateDiscountedPrice();

        Console.WriteLine($"Discounted price after adding discount: {room.DiscountedPrice}");

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new DiscountResponse(
            discount.Id,
            discount.DiscountPercentage,
            discount.StartDate,
            discount.EndDate
        );
    }
}
