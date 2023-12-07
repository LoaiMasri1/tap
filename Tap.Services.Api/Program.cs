using Serilog;
using Tap.Application;
using Tap.Infrastructure;
using Tap.Persistence;
using Tap.Services.Api;
using Tap.Services.Api.Extentions;

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

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigration();
}

app.MapControllers();

app.UseHttpsRedirection();

app.Run();
