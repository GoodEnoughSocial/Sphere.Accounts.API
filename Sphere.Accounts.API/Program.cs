using Serilog;
using Sphere.Shared;

// Setting this allows us to get some benefits all over the place.
Services.Current = Services.Accounts;

Log.Logger = SphericalLogger.StartupLogger(Services.Current);

try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Services.AddHealthChecks();
    builder.Services.AddInjectableOrleansClient();

    builder.Host.UseSerilog(SphericalLogger.SetupLogger);

    // Add services to the container.

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    app.MapHealthChecks(Constants.HealthCheckEndpoint);

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    if (ex.GetType().Name != "StopTheHostException")
    {
        Log.Fatal(ex, "Unhandled exception");
    }
}
finally
{
    Log.Information("Shutting down");
    Log.CloseAndFlush();
}
