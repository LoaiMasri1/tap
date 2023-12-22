using Microsoft.EntityFrameworkCore;
using Tap.Persistence;

namespace Tap.Services.Api.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication ApplyMigration(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<TapDbContext>();

        dbContext.Database.Migrate();
        return app;
    }

    public static WebApplication AddSwagger(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(
            c =>
                c.HeadContent = """
                
                        <style>
                    :root{
                        --background-color: #fff;
                        --text-color: #000;
                        --primary-color: #0673b3;
                        --secondary-color: #009688;
                        --danger-color: #f7412d;
                        --dark-color: #33495f;
                        --light-color: #f1f1f1;
                  
                
                        --primary-light-color: rgb(6, 115, 179, 0.1);
                        --secondary-light-color: rgb(0, 150, 136, 0.1);
                        --danger-light-color: rgb(247, 65, 45, 0.1);
                        --dark-light-color: rgb(51, 73, 95, 0.1);
                
                        --logo-url : url("https://www.foothillsolutions.com/static/logo-a1f0dd26b5e986c8b9d362aec4fad576.png");
                    }
                
                  .swagger-ui .topbar {
                    background-color: var(--background-color);
                    box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
                  }
                  .swagger-ui .topbar .link {
                    color: var(--text-color);
                  }
                  .swagger-ui .topbar .link img {
                    content: var(--logo-url);
                    height: 35px;
                  }
                
                  .swagger-ui .info .title small.version-stamp {
                    background-color: var(--primary-color);
                  }
                  .swagger-ui .topbar .download-url-wrapper .select-label select {
                    border: 2px solid var(--primary-color)
                  }
                
                  .swagger-ui .btn.authorize {
                    background-color: var(--primary-color);
                    color: var(--light-color);
                    border-color: var(--primary-color);
                  }
                
                  .swagger-ui .btn.authorize svg {
                    fill: var(--light-color);
                  }
                  .swagger-ui .opblock.opblock-post .opblock-summary-method {
                    background-color: var(--primary-color)
                  }
                
                  .swagger-ui .opblock.opblock-post {
                      border-color: var(--primary-color);
                      background: var(--primary-light-color);
                  }
                
                  .swagger-ui .opblock.opblock-get .opblock-summary-method {
                    background-color:var(--secondary-color);
                  }
                
                  .swagger-ui .opblock.opblock-get {
                    border-color:  var(--secondary-color);
                    background: var(--secondary-light-color);
                
                  }
                
                  .swagger-ui .opblock.opblock-delete .opblock-summary-method {
                    background-color: var(--danger-color);
                  }
                
                  .swagger-ui .opblock.opblock-delete {
                    border-color: var(--danger-color);
                    background: var(--danger-light-color);
                  }
                
                  .swagger-ui .opblock.opblock-put .opblock-summary-method {
                    background-color: var(--dark-color);
                  }
                
                  .swagger-ui .opblock.opblock-put {
                    border-color: var(--dark-color);
                    background: var(--dark-light-color);
                  }
                
                  .swagger-ui .response-control-media-type--accept-controller select {
                    border-color: var(--primary-color)
                  }
                  .swagger-ui .response-control-media-type__accept-message {
                    color: var(--primary-color)
                  }
                </style>
                """
        );

        return app;
    }
}
