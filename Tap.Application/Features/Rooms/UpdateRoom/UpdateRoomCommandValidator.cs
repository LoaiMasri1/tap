using FluentValidation;
using Tap.Application.Core.Errors;
using Tap.Application.Core.Extensions;

namespace Tap.Application.Features.Rooms.UpdateRoom;

public class UpdateRoomCommandValidator : AbstractValidator<UpdateRoomCommand>
{
    public UpdateRoomCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithError(ValidationErrors.UpdateRoom.IdIsRequired);

        RuleFor(x => x.Number).NotEmpty().WithError(ValidationErrors.UpdateRoom.NumberIsRequired);
        RuleFor(x => x.Price).NotEmpty().WithError(ValidationErrors.UpdateRoom.PriceIsRequired);
        RuleFor(x => x.Currency)
            .NotEmpty()
            .WithError(ValidationErrors.UpdateRoom.CurrencyIsRequired);
        RuleFor(x => x.Type).NotEmpty().WithError(ValidationErrors.UpdateRoom.TypeIsRequired);
        RuleFor(x => x.CapacityOfAdults)
            .NotEmpty()
            .WithError(ValidationErrors.UpdateRoom.CapacityOfAdultsIsRequired);
        RuleFor(x => x.CapacityOfChildren)
            .NotEmpty()
            .WithError(ValidationErrors.UpdateRoom.CapacityOfChildrenIsRequired);
    }
}
