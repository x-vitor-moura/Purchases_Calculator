using Purchases_Calculator.API.Infrastructure.Configuration;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("Starting web application");

    var builder = WebApplication.CreateBuilder(args);
    
    builder.AddServices();
    
    var app = builder.Build();
    
    app.Configure();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly!");
}
finally
{
    Log.CloseAndFlush();
}

// Make the implicit Program class public so test projects can access it
public partial class Program { }