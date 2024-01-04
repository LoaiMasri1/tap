using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;
using Tap.Application.Core.Abstractions.Common;
using Tap.Application.Core.Abstractions.Cryptography;
using Tap.Application.Core.Abstractions.Email;
using Tap.Application.Core.Abstractions.Notification;
using Tap.Application.Core.Abstractions.Sessions;
using Tap.Application.Features.Authentication;
using Tap.Domain.Common.Services;
using Tap.Domain.Features.Amenities;
using Tap.Infrastructure.Authentication;
using Tap.Infrastructure.Authentication.Options;
using Tap.Infrastructure.Common;
using Tap.Infrastructure.Common.Options;
using Tap.Infrastructure.Cryptography;
using Tap.Infrastructure.Emails;
using Tap.Infrastructure.Emails.Options;
using Tap.Infrastructure.Notification;
using Tap.Infrastructure.Payment;
using Tap.Infrastructure.Payment.Options;
using FileService = Tap.Infrastructure.Common.FileService;

namespace Tap.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration
    ) =>
        services
            .Configure<MailOptions>(configuration.GetSection(MailOptions.SectionName).Bind)
            .AddTransient<IDateTime, DateTimeProvider>()
            .AddTransient<IPasswordHasher, PasswordHasher>()
            .AddTransient<IPasswordHashChecker, PasswordHasher>()
            .AddTransient<IJwtProvider, JwtProvider>()
            .AddScoped<IEmailService, EmailService>()
            .AddScoped<ITokenGenerator, GuidTokenGenerator>()
            .AddScoped<IEmailNotificationService, EmailNotificationService>()
            .AddScoped<IUserContext, UserContext>()
            .AddTransient<IFileService, FileService>()
            .AddTransient<IUploadFileService, AzureBlobService>()
            .AddTransient<IAmenityService, AmenityService>()
            .AddAuthentication(configuration)
            .AddAzureBlob()
            .AddStripe(configuration);

    public static IServiceCollection AddAuthentication(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();

        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionKey));
        services.ConfigureOptions<ConfigureJwtBearerOptions>();

        services.AddAuthorization();

        return services;
    }

    public static IServiceCollection AddAzureBlob(this IServiceCollection services)
    {
        services.ConfigureOptions<ConfigureAzureBlobOptions>();

        services.AddSingleton(provider =>
        {
            var options = provider.GetRequiredService<IOptions<AzureBlobOptions>>().Value;
            return new BlobServiceClient(options.ConnectionString);
        });

        return services;
    }

    public static IServiceCollection AddStripe(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.Configure<StripeOptions>(configuration.GetSection(StripeOptions.SectionName));

        var options = services
            .BuildServiceProvider()
            .GetRequiredService<IOptions<StripeOptions>>()
            .Value;

        StripeConfiguration.ApiKey = options.SecretKey;

        services.AddScoped<SessionService>();
        services.AddScoped<ISessionService, StripeService>();

        return services;
    }
}
