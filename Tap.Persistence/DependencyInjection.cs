using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tap.Application.Core.Abstractions.Data;
using Tap.Domain.Features.Amenities;
using Tap.Domain.Features.Bookings;
using Tap.Domain.Features.Cities;
using Tap.Domain.Features.Hotels;
using Tap.Domain.Features.Photos;
using Tap.Domain.Features.Reviews;
using Tap.Domain.Features.Rooms;
using Tap.Domain.Features.Users;
using Tap.Persistence.Infrastructure;
using Tap.Persistence.Repositories;

namespace Tap.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var connectionString = configuration.GetConnectionString(ConnectionString.SectionName)!;

        services.AddSingleton(new ConnectionString(connectionString));

        services.AddDbContext<TapDbContext>(options => options.UseSqlServer(connectionString));

        services.AddScoped<IDbContext>(
            serviceProvider => serviceProvider.GetService<TapDbContext>()!
        );

        services.AddScoped<IUnitOfWork>(
            serviceProvider => serviceProvider.GetService<TapDbContext>()!
        );

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IHotelRepository, HotelRepository>();
        services.AddScoped<ICityRepository, CityRepository>();
        services.AddScoped<IPhotoRepository, PhotoRepository>();
        services.AddScoped<IAmenityRepository, AmenityRepository>();
        services.AddScoped<IRoomRepository, RoomRepository>();
        services.AddScoped<IReviewRepository, ReviewRepository>();
        services.AddScoped<IBookingRepository, BookingRepository>();

        return services;
    }
}
