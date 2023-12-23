using Serilog;
using Tap.Application;
using Tap.Infrastructure;
using Tap.Persistence;
using Tap.Services.Api;
using Tap.Services.Api.Extensions;
using Tap.Services.Api.Middlewares;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog(
    (context, configuration) => configuration.ReadFrom.Configuration(context.Configuration)
);

var configuration = builder.Configuration;

builder.Services
    .AddApi()
    .AddApplication()
    .AddInfrastructure(configuration)
    .AddPersistence(configuration);

var app = builder.Build();

app.UseMiddleware<ExceptionHandlerMiddleware>();

//if (app.Environment.IsDevelopment())
//{
app.AddSwagger();
app.ApplyMigration();

//}

app.MapControllers();

app.UseStaticFiles();

app.UseAuthentication();

app.UseAuthorization();

//app.UseHttpsRedirection();

app.Run();
