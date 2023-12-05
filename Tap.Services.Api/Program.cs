using Tap.Services.Api;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services.AddApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.UseHttpsRedirection();

app.Run();
