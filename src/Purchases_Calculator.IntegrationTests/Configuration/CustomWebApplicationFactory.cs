using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Purchases_Calculator.API.Infrastructure.Databases;

namespace Purchases_Calculator.IntegrationTests.Configuration;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        // Add mock/test services to the builder here
        builder.ConfigureServices(services =>
        {
            // Remove the existing AppDbContext registration to replace it with an in-memory database
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            // Add in-memory database for testing
            services.AddScoped(sp =>
            {
                return new DbContextOptionsBuilder<AppDbContext>()
                    .UseInMemoryDatabase("IntegrationTestDatabase") // You can use a unique name to isolate tests
                    .UseApplicationServiceProvider(sp)
                    .Options;
            });
        });

        return base.CreateHost(builder);
    }
}
