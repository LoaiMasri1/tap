﻿using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Tap.Application.Core.Abstractions.Common;
using Tap.Application.Core.Abstractions.Cryptography;
using Tap.Application.Core.Abstractions.Email;
using Tap.Application.Core.Abstractions.Notification;
using Tap.Application.Features.Authentication;
using Tap.Domain.Common.Services;
using Tap.Infrastructure.Authentication;
using Tap.Infrastructure.Authentication.Options;
using Tap.Infrastructure.Common;
using Tap.Infrastructure.Common.Options;
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
        services.AddTransient<IJwtProvider, JwtProvider>();

        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<ITokenGenerator, GuidTokenGenerator>();
        services.AddScoped<IEmailNotificationService, EmailNotificationService>();
        services.AddScoped<IUserIdentifierProvider, UserIdentifierProvider>();

        services.AddTransient<IFileService, FileService>();
        services.AddTransient<IUploadFileService, AzureBlobService>();

        services.AddAuthentication(configuration);

        services.AddAzureBlob();

        return services;
    }

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
}
