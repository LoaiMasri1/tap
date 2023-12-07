using Microsoft.EntityFrameworkCore;
using Tap.Persistence;

namespace Tap.Services.Api.Extentions;

public static class WebApplicationExtentions
{
    public static WebApplication ApplyMigration(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<TapDbContext>();

        dbContext.Database.Migrate();
        return app;
    }
}
