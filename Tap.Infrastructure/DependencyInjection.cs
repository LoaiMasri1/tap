using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tap.Application.Core.Abstractions.Common;
using Tap.Application.Core.Abstractions.Cryptography;
using Tap.Domain.Common.Services;
using Tap.Infrastructure.Common;
using Tap.Infrastructure.Cryptography;

namespace Tap.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddTransient<IDateTime, DateTimeProvider>();
        services.AddTransient<IPasswordHasher, PasswordHasher>();
        services.AddTransient<IPasswordHashChecker, PasswordHasher>();

        return services;
    }
}
