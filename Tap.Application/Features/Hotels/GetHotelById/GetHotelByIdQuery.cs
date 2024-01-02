using Tap.Application.Core.Messaging;
using Tap.Contracts.Features.Hotels;
using Tap.Domain.Core.Primitives.Maybe;

namespace Tap.Application.Features.Hotels.GetHotelById;

public record GetHotelByIdQuery(int Id, bool IncludeRooms) : IQuery<Maybe<SearchHotelResponse>>;
