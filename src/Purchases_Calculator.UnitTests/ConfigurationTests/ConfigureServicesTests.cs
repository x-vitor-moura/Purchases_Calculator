using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Purchases_Calculator.API.Infrastructure.Configuration;
using Purchases_Calculator.API.Infrastructure.Databases;
using Purchases_Calculator.API.Infrastructure.Databases.Repositories;
using Purchases_Calculator.API.Infrastructure.Messaging;

namespace Purchases_Calculator.UnitTests.ConfigurationTests;

public class ConfigureServicesTests
{
    [Fact]
    public void AddServices_RegistersRequiredServices()
    {
        // Arrange
        var builder = WebApplication.CreateBuilder(new string[] { });

        // Act
        builder.AddServices();

        var serviceProvider = builder.Services.BuildServiceProvider();

        // Assert
        var purchaseRepository = serviceProvider.GetService<IPurchaseRepository>();
        Assert.NotNull(purchaseRepository);

        var messagePublisher = serviceProvider.GetService<IMessagePublisher>();
        Assert.NotNull(messagePublisher);

        var dbContext = serviceProvider.GetService<AppDbContext>();
        Assert.NotNull(dbContext);
    }
}
