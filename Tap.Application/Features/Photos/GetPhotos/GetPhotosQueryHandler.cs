using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;
using Tap.Application.Core.Abstractions.Data;
using Tap.Application.Core.Messaging;
using Tap.Contracts.Features.Photos;
using Tap.Domain.Core.Primitives.Maybe;
using Tap.Domain.Features.Photos;

namespace Tap.Application.Features.Photos.GetPhotos
{
    public class GetPhotosQueryHandler : IQueryHandler<GetPhotosQuery, Maybe<GetPhotoResponse[]>>
    {
        private readonly IDbContext _dbContext;
        private readonly ISieveProcessor _sieveProcessor;

        public GetPhotosQueryHandler(IDbContext dbContext, ISieveProcessor sieveProcessor)
        {
            _dbContext = dbContext;
            _sieveProcessor = sieveProcessor;
        }

        public async Task<Maybe<GetPhotoResponse[]>> Handle(
            GetPhotosQuery request,
            CancellationToken cancellationToken
        )
        {
            var sieveModel = new SieveModel
            {
                Filters = request.Filters,
                Page = request.Page,
                PageSize = request.PageSize
            };

            var photos = _dbContext.Set<Photo>().AsNoTracking();

            var photosPaged = await _sieveProcessor
                .Apply(sieveModel, photos)
                .Select(x => new GetPhotoResponse(x.Url, x.ItemId, x.Type))
                .ToArrayAsync(cancellationToken);

            return photosPaged;
        }
    }
}
