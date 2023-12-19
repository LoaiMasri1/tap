namespace Tap.Domain.Features.Photos;

public interface IPhotoRepository
{
    void Insert(Photo photo);
    void InsertRange(IReadOnlyCollection<Photo> photos);
}
