using Sieve.Services;
using Tap.Domain.Features.Photos;

namespace Tap.Application.Features.Photos
{
    public class PhotoSieveConfiguration : ISieveConfiguration
    {
        public void Configure(SievePropertyMapper mapper)
        {
            mapper.Property<Photo>(p => p.Id).CanFilter();
            mapper.Property<Photo>(p => p.Type).CanFilter();
            mapper.Property<Photo>(p => p.ItemId).CanFilter();
        }
    }
}
