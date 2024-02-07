using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Tap.Infrastructure.Common.Options;

public class ConfigureAzureBlobOptions : IConfigureOptions<AzureBlobOptions>
{
    private readonly IConfiguration _configuration;

    public ConfigureAzureBlobOptions(IConfiguration configuration) =>
        _configuration = configuration;

    public void Configure(AzureBlobOptions options)
    {
        options.ConnectionString = _configuration.GetConnectionString(
            AzureBlobOptions.SectionName
        )!;
        _configuration.GetSection(AzureBlobOptions.SectionName).Bind(options);
    }
}
