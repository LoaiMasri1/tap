using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tap.Application.Core.Abstractions.Common;
using Tap.Infrastructure.Common;

namespace Tap.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddTransient<IDateTime, DateTimeProvider>();

        return services;
    }
}
