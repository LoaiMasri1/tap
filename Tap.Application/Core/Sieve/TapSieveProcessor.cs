using Microsoft.Extensions.Options;
using Sieve.Models;
using Sieve.Services;

namespace Tap.Application.Core.Sieve;

public class TapSieveProcessor : SieveProcessor
{
    public TapSieveProcessor(IOptions<SieveOptions> options)
        : base(options) { }

    protected override SievePropertyMapper MapProperties(SievePropertyMapper mapper) =>
        mapper.ApplyConfigurationsFromAssembly(typeof(TapSieveProcessor).Assembly);
}
