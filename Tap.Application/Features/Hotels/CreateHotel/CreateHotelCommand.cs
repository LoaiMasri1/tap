using Tap.Application.Core.Messaging;
using Tap.Contracts.Features.Hotels;
using Tap.Domain.Core.Primitives.Result;

namespace Tap.Application.Features.Hotels.CreateHotel;

public record CreateHotelCommand(
    string Name,
    string Description,
    double Latitude,
    double Longitude,
    int CityId
) : ICommand<Result<HotelResponse>>;
