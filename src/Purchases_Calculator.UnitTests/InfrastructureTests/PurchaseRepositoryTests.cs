using Microsoft.EntityFrameworkCore;
using Purchases_Calculator.API.Domain;
using Purchases_Calculator.API.Infrastructure.Databases;
using Purchases_Calculator.API.Infrastructure.Databases.Repositories;

namespace Purchases_Calculator.UnitTests.InfrastructureTests;

public class PurchaseRepositoryTests
{
    private readonly DbContextOptions<AppDbContext> _options;

    public PurchaseRepositoryTests()
    {
        _options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;
    }

    [Fact]
    public async Task CreateAsync_SavesPurchaseCorrectly()
    {
        using var context = new AppDbContext(_options);
        var repository = new PurchaseRepository(context);

        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        // Act
        var purchase = await CreatePurchaseAsync(repository, 100m, 120m, 20m, 20);

        // Assert
        Assert.True(purchase.Id > 0);  // Check that the ID was auto-generated
        Assert.Equal(100m, purchase.Net);
        Assert.Equal(120m, purchase.Gross);
        Assert.Equal(20m, purchase.Vat);
        Assert.Equal(20, purchase.VatRate);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsCorrectPurchase()
    {
        // Arrange
        using var context = new AppDbContext(_options);
        var repository = new PurchaseRepository(context);

        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        var purchase = await CreatePurchaseAsync(repository, 100m, 120m, 20m, 20);

        // Act
        var retrievedPurchase = await GetPurchaseByIdAsync(repository, purchase.Id);

        // Assert
        Assert.NotNull(retrievedPurchase);
        Assert.Equal(purchase.Id, retrievedPurchase.Id);
        Assert.Equal(100m, retrievedPurchase.Net);
        Assert.Equal(120m, retrievedPurchase.Gross);
        Assert.Equal(20m, retrievedPurchase.Vat);
        Assert.Equal(20, retrievedPurchase.VatRate);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllPurchases()
    {
        // Arrange
        using var context = new AppDbContext(_options);
        var repository = new PurchaseRepository(context);

        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        var purchase1 = await CreatePurchaseAsync(repository, 100m, 120m, 20m, 20);
        var purchase2 = await CreatePurchaseAsync(repository, 200m, 240m, 40m, 20);

        // Act
        var allPurchases = await GetAllPurchasesAsync(repository);

        // Assert
        Assert.Equal(2, allPurchases.Count());
        Assert.Contains(allPurchases, p => p.Id == purchase1.Id);
        Assert.Contains(allPurchases, p => p.Id == purchase2.Id);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenPurchaseNotFound()
    {
        // Arrange
        using var context = new AppDbContext(_options);
        var repository = new PurchaseRepository(context);

        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        // Act
        var nonExistentPurchase = await GetPurchaseByIdAsync(repository, 999);

        // Assert
        Assert.Null(nonExistentPurchase);
    }

    private async Task<Purchase> CreatePurchaseAsync(PurchaseRepository repository, decimal net, decimal gross, decimal vat, int vatRate)
    {
        var purchase = new Purchase(net, gross, vat, vatRate);
        await repository.CreateAsync(purchase);
        return purchase;
    }

    private async Task<Purchase> GetPurchaseByIdAsync(PurchaseRepository repository, int id)
    {
        return await repository.GetByIdAsync(id);
    }

    private async Task<IEnumerable<Purchase>> GetAllPurchasesAsync(PurchaseRepository repository)
    {
        return await repository.GetAllAsync();
    }
}