using Tap.Application.Core.Messaging;
using Tap.Contracts.Features.Cities;
using Tap.Domain.Core.Primitives.Result;

namespace Tap.Application.Features.Cities.CreateCity;

public record CreateCityCommand(string Name, string Description, string Country)
    : ICommand<Result<CityResponse>>;
