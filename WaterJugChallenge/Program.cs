using Microsoft.OpenApi.Models;
using Serilog;
using WaterJugChallenge.Services;
using WebApi.Middlewares;

public class Program  // Change this to public
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Register services
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();

        // Configure Swagger for API documentation
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Water Jug API", Version = "v1" });
        });

        // Register application services
        builder.Services.AddScoped<IWaterJugRiddleService, WaterJugRiddleService>();
        builder.Services.AddMemoryCache();

        // Configure Serilog with file and console sinks
        var logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)  // Read from appsettings.json
            .Enrich.FromLogContext()
            .CreateLogger();

        builder.Logging.ClearProviders();
        builder.Logging.AddSerilog(logger);

        // Build application
        var app = builder.Build();

        // Middleware configuration
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthorization();

        // Enable Swagger UI in development environment
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Water Jug API V1");
                c.RoutePrefix = string.Empty;
            });
        }

        // Add custom error handling middleware
        app.UseMiddleware<ErrorHandlerMiddleware>();
        app.UseMiddleware<LoggingMiddleware>();

        // Map controllers
        app.MapControllers();

        // Run the application
        app.Run();
    }
}