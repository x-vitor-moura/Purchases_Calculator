using Purchases_Calculator.API.Common.Middlewares;
using Purchases_Calculator.API.Endpoints;
using Serilog;

namespace Purchases_Calculator.API.Infrastructure.Configuration;

public static class ConfigureApp
{
    public static async Task Configure(this WebApplication app)
    {
        app.UseMiddleware<BadRequestHandlingMiddleware>();

        app.UseSerilogRequestLogging();
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseHttpsRedirection();
        app.MapEndpoints();
    }
}
