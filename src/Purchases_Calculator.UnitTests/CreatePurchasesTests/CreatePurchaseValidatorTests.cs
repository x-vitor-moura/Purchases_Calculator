using Purchases_Calculator.API.Endpoints.Requests;
using Purchases_Calculator.API.Endpoints.Validators;

namespace Purchases_Calculator.UnitTests.CreatePurchasesTests;

public class CreatePurchaseValidatorTests
{
    [Fact]
    public void Validate_AllFieldsEmpty_ShouldFail()
    {
        // Arrange
        var validator = new CreatePurchaseValidator();
        var request = new CreatePurchaseRequest(null, null, null, 20);

        // Act
        var validationResult = validator.Validate(request);

        // Assert
        Assert.False(validationResult.IsValid);
        Assert.Contains(validationResult.Errors, e => e.ErrorMessage == "Exactly one of 'Net', 'Gross', or 'Vat' must be provided.");
    }

    [Fact]
    public void Validate_MultipleAmountsFilled_ShouldFail()
    {
        // Arrange
        var validator = new CreatePurchaseValidator();
        var request = new CreatePurchaseRequest(100, 200, null, 20);

        // Act
        var validationResult = validator.Validate(request);

        // Assert
        Assert.False(validationResult.IsValid);
        Assert.Contains(validationResult.Errors, e => e.ErrorMessage == "Exactly one of 'Net', 'Gross', or 'Vat' must be provided.");
    }

    [Fact]
    public void Validate_Net_ValidValue_ShouldPass()
    {
        // Arrange
        var validator = new CreatePurchaseValidator();
        var request = new CreatePurchaseRequest(100, null, null, 20);

        // Act
        var validationResult = validator.Validate(request);

        // Assert
        Assert.True(validationResult.IsValid);
    }

    [Fact]
    public void Validate_Gross_ValidValue_ShouldPass()
    {
        // Arrange
        var validator = new CreatePurchaseValidator();
        var request = new CreatePurchaseRequest(null, 120, null, 20);
        // Act
        var validationResult = validator.Validate(request);

        // Assert
        Assert.True(validationResult.IsValid);
    }

    [Fact]
    public void Validate_Vat_ValidValue_ShouldPass()
    {
        // Arrange
        var validator = new CreatePurchaseValidator();
        var request = new CreatePurchaseRequest(null, null, 20, 20);

        // Act
        var validationResult = validator.Validate(request);

        // Assert
        Assert.True(validationResult.IsValid);
    }

    [Fact]
    public void Validate_Net_Zero_ShouldFail()
    {
        // Arrange
        var validator = new CreatePurchaseValidator();
        var request = new CreatePurchaseRequest(0, null, null, 20);
        // Act
        var validationResult = validator.Validate(request);

        // Assert
        Assert.False(validationResult.IsValid);
        Assert.Contains(validationResult.Errors, e => e.PropertyName == nameof(CreatePurchaseRequest.Net) && e.ErrorMessage == "'Net' must be greater than 0.");
    }

    [Fact]
    public void Validate_Gross_Zero_ShouldFail()
    {
        // Arrange
        var validator = new CreatePurchaseValidator();
        var request = new CreatePurchaseRequest(null, 0, null, 20);

        // Act
        var validationResult = validator.Validate(request);

        // Assert
        Assert.False(validationResult.IsValid);
        Assert.Contains(validationResult.Errors, e => e.PropertyName == nameof(CreatePurchaseRequest.Gross) && e.ErrorMessage == "'Gross' must be greater than 0.");
    }

    [Fact]
    public void Validate_Vat_Zero_ShouldFail()
    {
        // Arrange
        var validator = new CreatePurchaseValidator();
        var request = new CreatePurchaseRequest(null, null, 0, 20);

        // Act
        var validationResult = validator.Validate(request);

        // Assert
        Assert.False(validationResult.IsValid);
        Assert.Contains(validationResult.Errors, e => e.PropertyName == nameof(CreatePurchaseRequest.Vat) && e.ErrorMessage == "'Vat' must be greater than 0.");
    }

    [Fact]
    public void Validate_VatRate_InvalidValue_ShouldFail()
    {
        // Arrange
        var validator = new CreatePurchaseValidator();
        var request = new CreatePurchaseRequest(100, null, null, 15);

        // Act
        var validationResult = validator.Validate(request);

        // Assert
        Assert.False(validationResult.IsValid);
        Assert.Contains(validationResult.Errors, e => e.PropertyName == nameof(CreatePurchaseRequest.VatRate) && e.ErrorMessage == "'VatRate' must be 10, 13, or 20.");
    }

    [Fact]
    public void Validate_VatRate_ValidValue_ShouldPass()
    {
        // Arrange
        var validator = new CreatePurchaseValidator();
        var request = new CreatePurchaseRequest(100, null, null, 20);

        // Act
        var validationResult = validator.Validate(request);

        // Assert
        Assert.True(validationResult.IsValid);
    }
}
