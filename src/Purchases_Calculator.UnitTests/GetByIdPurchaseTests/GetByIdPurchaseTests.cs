using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using Purchases_Calculator.API.Domain;
using Purchases_Calculator.API.Endpoints.APIs;
using Purchases_Calculator.API.Endpoints.Requests;
using Purchases_Calculator.API.Endpoints.Responses;
using Purchases_Calculator.API.Infrastructure.Databases.Repositories;

namespace Purchases_Calculator.UnitTests.GetByIdPurchaseTests;

public class GetByIdPurchaseTests
{
    private readonly Mock<IPurchaseRepository> _mockPurchaseRepository;

    public GetByIdPurchaseTests()
    {
        _mockPurchaseRepository = new Mock<IPurchaseRepository>();
    }

    [Fact]
    public async Task Handle_ValidId_ReturnsOk()
    {
        // Arrange
        var purchaseId = 1;
        var purchase = new Purchase(100m, 120m, 20m, 20)
        {
            Id = purchaseId
        };

        var request = new GetByIdPurchaseRequest(purchaseId);

        _mockPurchaseRepository.Setup(repo => repo.GetByIdAsync(purchaseId))
            .ReturnsAsync(purchase);


        // Act
        var result = await GetByIdPurchase.Handle(request, _mockPurchaseRepository.Object, CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<Ok<GetByIdPurchaseDetails>>(result);
        var okValue = okResult.Value;

        Assert.Equal(1, okValue.Id);
        Assert.Equal(100, okValue.Net);
        Assert.Equal(120, okValue.Gross);
        Assert.Equal(20, okValue.Vat);
        Assert.Equal(20, okValue.VatRate);
        Assert.Equal(purchase.PurchaseRegistrationDate.ToString("yyyy-MM-dd HH:mm"), okValue.PurchaseRegistrationDate);
    }

    [Fact]
    public async Task Handle_InvalidId_ReturnsNotFound()
    {
        // Arrange
        var invalidId = 999;

        _mockPurchaseRepository.Setup(repo => repo.GetByIdAsync(invalidId))
            .ReturnsAsync((Purchase)null); // Simulate not found

        var request = new GetByIdPurchaseRequest(invalidId);

        // Act
        var result = await GetByIdPurchase.Handle(request, _mockPurchaseRepository.Object, CancellationToken.None);

        // Assert
        Assert.IsType<NotFound<string>>(result);
    }
}