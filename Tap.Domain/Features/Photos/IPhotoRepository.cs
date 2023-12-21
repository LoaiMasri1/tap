using Tap.Domain.Core.Primitives.Maybe;

namespace Tap.Domain.Features.Photos;

public interface IPhotoRepository
{
    void Insert(Photo photo);
    void InsertRange(IReadOnlyCollection<Photo> photos);
    Task<Maybe<Photo>> GetByIdAsync(int id, CancellationToken cancellationToken);
    void Remove(Photo photo);
}
