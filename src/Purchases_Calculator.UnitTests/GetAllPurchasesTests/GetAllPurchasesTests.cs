using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Moq;
using Purchases_Calculator.API.Domain;
using Purchases_Calculator.API.Endpoints.APIs;
using Purchases_Calculator.API.Endpoints.Responses;
using Purchases_Calculator.API.Infrastructure.Databases.Repositories;
using Purchases_Calculator.API.Infrastructure.Databases;

namespace Purchases_Calculator.UnitTests.GetAllPurchasesTests;

public class GetAllPurchasesTests
{
    private readonly Mock<IPurchaseRepository> _mockPurchaseRepository;

    public GetAllPurchasesTests()
    {
        _mockPurchaseRepository = new Mock<IPurchaseRepository>();
    }

    [Fact]
    public async Task ShouldGenerateId_WhenItemIsSaved()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
        .Options;

        using (var context = new AppDbContext(options))
        {
            var purchase = new Purchase(200, 240, 40, 20);

            context.Add(purchase);
            await context.SaveChangesAsync();

            // Assert
            Assert.True(purchase.Id > 0);  // Check if ID was auto-generated
        }
    }

    [Fact]
    public async Task Handle_ReturnsOk_WithPurchases()
    {
        // Arrange
        var purchases = new List<Purchase>
        {
            new Purchase(100, 120, 20, 20){Id = 1},
            new Purchase(200, 240, 40, 20){Id = 2}
        };


        _mockPurchaseRepository.Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(purchases);

        // Act
        var result = await GetAllPurchase.Handle(_mockPurchaseRepository.Object, default);

        // Assert
        var okResult = Assert.IsType<Ok<IEnumerable<GetAllPurchaseDto>>>(result);
        var purchaseDtos = Assert.IsAssignableFrom<IEnumerable<GetAllPurchaseDto>>(okResult.Value);

        Assert.Equal(2, purchaseDtos.Count());
        Assert.Equal(1, purchaseDtos.ElementAt(0).Id);
        Assert.Equal(100, purchaseDtos.ElementAt(0).Net);
        Assert.Equal(120, purchaseDtos.ElementAt(0).Gross);
        Assert.Equal(20, purchaseDtos.ElementAt(0).Vat);
        Assert.Equal(20, purchaseDtos.ElementAt(0).VatRate);
    }

    [Fact]
    public async Task Handle_ReturnsOk_WithNoPurchases()
    {
        // Arrange
        _mockPurchaseRepository.Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(new List<Purchase>());

        // Act
        var result = await GetAllPurchase.Handle(_mockPurchaseRepository.Object, default);

        // Assert
        var okResult = Assert.IsType<Ok<IEnumerable<GetAllPurchaseDto>>>(result);
        var purchaseDtos = Assert.IsAssignableFrom<IEnumerable<GetAllPurchaseDto>>(okResult.Value);


        Assert.Empty(purchaseDtos);
    }
}
