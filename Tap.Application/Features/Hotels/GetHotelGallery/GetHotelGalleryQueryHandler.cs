using Microsoft.EntityFrameworkCore;
using Tap.Application.Core.Abstractions.Data;
using Tap.Application.Core.Extensions;
using Tap.Application.Core.Messaging;
using Tap.Contracts.Features.Photos;
using Tap.Domain.Core.Primitives.Maybe;
using Tap.Domain.Features.Photos;
using Tap.Domain.Features.Rooms;

namespace Tap.Application.Features.Hotels.GetHotelGallery;

public class GetHotelGalleryQueryHandler
    : IQueryHandler<GetHotelGalleryQuery, Maybe<PhotoResponse[]>>
{
    private readonly IDbContext _dbContext;

    public GetHotelGalleryQueryHandler(IDbContext dbContext) => _dbContext = dbContext;

    public async Task<Maybe<PhotoResponse[]>> Handle(
        GetHotelGalleryQuery request,
        CancellationToken cancellationToken
    )
    {
        var entityIds = await _dbContext
            .Set<Room>()
            .AsNoTracking()
            .Where(r => r.HotelId == request.Id)
            .Paginate(request.PageNumber, request.PageSize)
            .Select(r => r.Id)
            .ToListAsync(cancellationToken);

        entityIds.Add(request.Id);

        var photos = _dbContext.Set<Photo>().AsNoTracking();

        var gallery = await photos
            .Where(p => entityIds.Contains(p.ItemId))
            .Select(p => new PhotoResponse(p.Url))
            .ToArrayAsync(cancellationToken);

        return gallery;
    }
}
