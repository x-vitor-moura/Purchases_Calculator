using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using Purchases_Calculator.API.Domain;
using Purchases_Calculator.API.Endpoints.APIs;
using Purchases_Calculator.API.Endpoints.Requests;
using Purchases_Calculator.API.Endpoints.Responses;
using Purchases_Calculator.API.Infrastructure.Databases.Repositories;
using Purchases_Calculator.API.Infrastructure.Messaging;

namespace Purchases_Calculator.UnitTests.CreatePurchasesTests;

public class CreatePurchaseTests
{
    private readonly Mock<IMessagePublisher> _mockMessagePublisher;
    private readonly Mock<IPurchaseRepository> _mockPurchaseRepository;

    public CreatePurchaseTests()
    {
        _mockMessagePublisher = new Mock<IMessagePublisher>();
        _mockPurchaseRepository = new Mock<IPurchaseRepository>();
    }

    [Fact]
    public async Task Handle_ValidRequestNetAndVatRate_ReturnsOk()
    {
        // Arrange
        var validRequest = new CreatePurchaseRequest(100m, null, null, 20);
        var mockPurchase = CreateMockPurchase(); 

        _mockPurchaseRepository.Setup(x => x.CreateAsync(It.IsAny<Purchase>())).Returns(Task.CompletedTask);
        _mockMessagePublisher.Setup(x => x.PublishMessageAsync("purchase",It.IsAny<Purchase>())).Returns(Task.CompletedTask);

        // Act
        var result = await CreatePurchase.Handle(validRequest, _mockMessagePublisher.Object, _mockPurchaseRepository.Object, CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<Created<CreatePurchaseDto>>(result); 
        var returnValue = okResult.Value;

        // Validate the response content
        Assert.Equal(mockPurchase.Net, returnValue.Net);
        Assert.Equal(mockPurchase.Gross, returnValue.Gross);
        Assert.Equal(mockPurchase.Vat, returnValue.Vat);
        Assert.Equal(mockPurchase.VatRate, returnValue.VatRate);

        // Verify that repository and message publisher were called
        _mockPurchaseRepository.Verify(x => x.CreateAsync(It.IsAny<Purchase>()), Times.Once);
        _mockMessagePublisher.Verify(x => x.PublishMessageAsync("purchase",It.IsAny<Purchase>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ValidRequestGrossAndVatRate_ReturnsOk()
    {
        // Arrange
        var validRequest = new CreatePurchaseRequest(null, 120m, null, 20); 
        var mockPurchase = CreateMockPurchase();

        _mockPurchaseRepository.Setup(x => x.CreateAsync(It.IsAny<Purchase>())).Returns(Task.CompletedTask);
        _mockMessagePublisher.Setup(x => x.PublishMessageAsync("purchase", It.IsAny<Purchase>())).Returns(Task.CompletedTask);

        // Act
        var result = await CreatePurchase.Handle(validRequest, _mockMessagePublisher.Object, _mockPurchaseRepository.Object, CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<Created<CreatePurchaseDto>>(result);
        var returnValue = okResult.Value;

        // Validate the response content
        Assert.Equal(mockPurchase.Net, returnValue.Net);
        Assert.Equal(mockPurchase.Gross, returnValue.Gross);
        Assert.Equal(mockPurchase.Vat, returnValue.Vat);
        Assert.Equal(mockPurchase.VatRate, returnValue.VatRate);

        // Verify that repository and message publisher were called
        _mockPurchaseRepository.Verify(x => x.CreateAsync(It.IsAny<Purchase>()), Times.Once);
        _mockMessagePublisher.Verify(x => x.PublishMessageAsync("purchase", It.IsAny<Purchase>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ValidRequestVatAndVatRate_ReturnsOk()
    {
        // Arrange
        var validRequest = new CreatePurchaseRequest(null, null, 20, 20);
        var mockPurchase = CreateMockPurchase();

        _mockPurchaseRepository.Setup(x => x.CreateAsync(It.IsAny<Purchase>())).Returns(Task.CompletedTask);
        _mockMessagePublisher.Setup(x => x.PublishMessageAsync("purchase", It.IsAny<Purchase>())).Returns(Task.CompletedTask);

        // Act
        var result = await CreatePurchase.Handle(validRequest, _mockMessagePublisher.Object, _mockPurchaseRepository.Object, CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<Created<CreatePurchaseDto>>(result);
        var returnValue = okResult.Value;

        // Validate the response content
        Assert.Equal(mockPurchase.Net, returnValue.Net);
        Assert.Equal(mockPurchase.Gross, returnValue.Gross);
        Assert.Equal(mockPurchase.Vat, returnValue.Vat);
        Assert.Equal(mockPurchase.VatRate, returnValue.VatRate);

        // Verify that repository and message publisher were called
        _mockPurchaseRepository.Verify(x => x.CreateAsync(It.IsAny<Purchase>()), Times.Once);
        _mockMessagePublisher.Verify(x => x.PublishMessageAsync("purchase", It.IsAny<Purchase>()), Times.Once);
    }

    private Purchase CreateMockPurchase()
    {
        return new Purchase(100m, 120m, 20m, 20);
    }
}
