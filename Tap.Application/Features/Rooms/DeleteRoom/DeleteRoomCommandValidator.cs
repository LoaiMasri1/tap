using FluentValidation;
using Tap.Application.Core.Errors;
using Tap.Application.Core.Extensions;

namespace Tap.Application.Features.Rooms.DeleteRoom;

public class DeleteRoomCommandValidator : AbstractValidator<DeleteRoomCommand>
{
    public DeleteRoomCommandValidator()
    {
        RuleFor(c => c.Id)
            .GreaterThan(0)
            .NotEmpty()
            .WithError(ValidationErrors.DeleteRoom.RoomIdIsRequired);
    }
}
