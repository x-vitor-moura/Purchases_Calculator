using Purchases_Calculator.API.Domain;
using Purchases_Calculator.API.Endpoints.Requests;
using Purchases_Calculator.API.Endpoints.Responses;
using Purchases_Calculator.API.Infrastructure.Databases;
using Purchases_Calculator.IntegrationTests.Configuration;
using System.Net;
using System.Text.Json;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Purchases_Calculator.IntegrationTests;

public class PurchaseEndpointsTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public PurchaseEndpointsTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetAllPurchases_ShouldReturnEmpty_WhenNoPurchasesExist()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        await ClearDataBase(dbContext);

        // Act
        using var client = _factory.CreateClient();
        var response = await client.GetAsync("/gb/purchase");

        // Assert
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        Assert.Contains("[]", responseContent);
    }



    [Fact]
    public async Task GetAllPurchases_ShouldReturnPurchases_WhenDataExists()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        // Ensure the database is empty before inserting data
        await ClearDataBase(dbContext);


        var testPurchase = new Purchase(100, 120, 20, 20);
        dbContext.Purchases.Add(testPurchase);
        await dbContext.SaveChangesAsync();

        // Act
        using var client = _factory.CreateClient();
        var response = await client.GetAsync("/gb/purchase");

        // Assert
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();

        var purchases = JsonSerializer.Deserialize<List<GetAllPurchaseDto>>(responseContent,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        Assert.NotNull(purchases);
        Assert.Single(purchases);

        var firstPurchase = purchases[0];
        Assert.Equal(100m, firstPurchase.Net);
        Assert.Equal(120m, firstPurchase.Gross);
        Assert.Equal(20m, firstPurchase.Vat);
        Assert.Equal(20, firstPurchase.VatRate);
    }

    [Fact]
    public async Task GetPurchaseById_ShouldReturnPurchase_WhenDataExists()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        await ClearDataBase(dbContext);

        // Insert a test purchase
        var testPurchase = new Purchase(100, 120, 20, 20) { Id = 1 };
        dbContext.Purchases.Add(testPurchase);
        await dbContext.SaveChangesAsync();

        // Act
        using var client = _factory.CreateClient();
        var response = await client.GetAsync($"/gb/purchase/{testPurchase.Id}");

        // Assert
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();

        var purchase = JsonSerializer.Deserialize<GetByIdPurchaseDetails>(responseContent,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        Assert.NotNull(purchase);
        Assert.Equal(testPurchase.Net, purchase.Net);
        Assert.Equal(testPurchase.Gross, purchase.Gross);
        Assert.Equal(testPurchase.Vat, purchase.Vat);
        Assert.Equal(testPurchase.VatRate, purchase.VatRate);
    }


    [Fact]
    public async Task CreatePurchase_ShouldReturnPurchase_WhenValidDataIsSent()
    {
        // Arrange
        var newPurchase = new CreatePurchaseRequest(100, null, null, 20);
        var content = new StringContent(
            JsonSerializer.Serialize(newPurchase),
            Encoding.UTF8,
            "application/json"
        );

        using var client = _factory.CreateClient();

        // Act
        var response = await client.PostAsync("/gb/purchase", content);

        // Assert
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();

        var purchaseDto = JsonSerializer.Deserialize<CreatePurchaseDto>(responseContent,
                                                                        new JsonSerializerOptions
                                                                        {
                                                                            PropertyNameCaseInsensitive = true
                                                                        });

        Assert.NotNull(purchaseDto);
        Assert.Equal(100, purchaseDto.Net);
    }

    [Fact]
    public async Task CreatePurchase_ShouldReturnBadRequest_WhenInvalidDataIsSent()
    {
        // Arrange
        var newPurchase = new CreatePurchaseInvalidRequest("asda", null, null, 20);

        var content = new StringContent(
            JsonSerializer.Serialize(newPurchase),
            Encoding.UTF8,
            "application/json"
        );

        using var client = _factory.CreateClient();

        // Act
        var response = await client.PostAsync("/gb/purchase", content);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var responseContent = await response.Content.ReadAsStringAsync();
        Assert.Contains("Invalid JSON request", responseContent);

        var errorDetails = JsonSerializer.Deserialize<BadRequestErrorResponse>(responseContent);
        Assert.NotNull(errorDetails);
        Assert.Equal(400, errorDetails.Status);
        Assert.Contains("'Net' must be numeric and greater the 0.", errorDetails.Detail);
    }

    private static async Task ClearDataBase(AppDbContext dbContext)
    {
        dbContext.Purchases.RemoveRange(dbContext.Purchases);
        await dbContext.SaveChangesAsync();
    }
    private record CreatePurchaseInvalidRequest(string? Net, decimal? Gross, decimal? Vat, int VatRate);
}
