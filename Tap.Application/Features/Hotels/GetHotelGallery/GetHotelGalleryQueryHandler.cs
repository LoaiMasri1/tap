using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;
using Tap.Application.Core.Abstractions.Data;
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
    private readonly ISieveProcessor _sieveProcessor;

    public GetHotelGalleryQueryHandler(IDbContext dbContext, ISieveProcessor sieveProcessor)
    {
        _dbContext = dbContext;
        _sieveProcessor = sieveProcessor;
    }

    public async Task<Maybe<PhotoResponse[]>> Handle(
        GetHotelGalleryQuery request,
        CancellationToken cancellationToken
    )
    {
        var sieveModel = new SieveModel { Page = request.Page, PageSize = request.PageSize, };

        var roomIds = _dbContext.Set<Room>().AsNoTracking().Where(r => r.HotelId == request.Id);

        var entityIds = await _sieveProcessor
            .Apply(sieveModel, roomIds)
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
