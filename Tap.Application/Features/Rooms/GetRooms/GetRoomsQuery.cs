using Tap.Application.Core.Messaging;
using Tap.Contracts.Features.Rooms;
using Tap.Domain.Common.QueryOptions;
using Tap.Domain.Core.Primitives.Maybe;

namespace Tap.Application.Features.Rooms.GetRooms;

public record GetRoomsQuery(int Page, int PageSize, string Sorts, string Filters)
    : IQuery<Maybe<FilteredRoomResponse[]>>,
        IPageable,
        ISortable,
        IFilterable;
