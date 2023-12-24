using Tap.Application.Core.Messaging;
using Tap.Domain.Core.Primitives.Result;

namespace Tap.Application.Features.Hotels.UpdateHotel;

public record UpdateHotelCommand(
    int Id,
    string Name,
    string Description,
    double Latitude,
    double Longitude
) : ICommand<Result>;
