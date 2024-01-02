using FluentValidation;
using Tap.Application.Core.Errors;
using Tap.Application.Core.Extensions;

namespace Tap.Application.Features.Rooms.CreateRoom;

public class CreateRoomCommandValidator : AbstractValidator<CreateRoomCommand>
{
    public CreateRoomCommandValidator()
    {
        RuleFor(x => x.HotelId).NotEmpty().WithError(ValidationErrors.CreateRoom.HotelIdIsRequired);
        RuleFor(x => x.Number).NotEmpty().WithError(ValidationErrors.CreateRoom.NumberIsRequired);
        RuleFor(x => x.Price).NotEmpty().WithError(ValidationErrors.CreateRoom.PriceIsRequired);
        RuleFor(x => x.Currency)
            .NotEmpty()
            .WithError(ValidationErrors.CreateRoom.CurrencyIsRequired);
        RuleFor(x => x.Type)
            .NotEmpty()
            .WithError(ValidationErrors.CreateRoom.TypeIsRequired)
            .IsInEnum()
            .WithError(ValidationErrors.CreateRoom.TypeIsInvalid);
        RuleFor(x => x.CapacityOfAdults)
            .NotEmpty()
            .WithError(ValidationErrors.CreateRoom.CapacityOfAdultsIsRequired);

        RuleFor(x => x.CapacityOfChildren)
            .GreaterThanOrEqualTo(0)
            .WithError(ValidationErrors.CreateRoom.CapacityOfChildrenIsRequired);
    }
}
