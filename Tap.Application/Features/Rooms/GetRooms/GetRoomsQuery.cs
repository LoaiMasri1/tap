using Tap.Application.Core.Messaging;
using Tap.Contracts.Features.Rooms;
using Tap.Domain.Core.Primitives.Maybe;

namespace Tap.Application.Features.Rooms.GetRooms;

public record GetRoomsQuery(
    bool IsAvailable,
    int PageNumber,
    int PageSize,
    string SortOrder,
    string SortBy,
    string? FilterBy,
    string? FilterQuery
) : IQuery<Maybe<FilteredRoomResponse[]>>, IPageable, ISortable, IFilterable;
