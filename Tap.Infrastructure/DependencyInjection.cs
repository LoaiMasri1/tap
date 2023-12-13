using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tap.Application.Core.Abstractions.Common;
using Tap.Application.Core.Abstractions.Cryptography;
using Tap.Application.Core.Abstractions.Email;
using Tap.Application.Core.Abstractions.Notification;
using Tap.Domain.Common.Services;
using Tap.Infrastructure.Common;
using Tap.Infrastructure.Cryptography;
using Tap.Infrastructure.Emails;
using Tap.Infrastructure.Emails.Options;
using Tap.Infrastructure.Notification;

namespace Tap.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.Configure<MailOptions>(configuration.GetSection(MailOptions.SectionName).Bind);

        services.AddTransient<IDateTime, DateTimeProvider>();
        services.AddTransient<IPasswordHasher, PasswordHasher>();
        services.AddTransient<IPasswordHashChecker, PasswordHasher>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<ITokenGenerator, GuidTokenGenerator>();

        services.AddScoped<IEmailNotificationService, EmailNotificationService>();

        return services;
    }
}
