namespace Purchases_Calculator.API.Endpoints.Responses;

public record CreatePurchaseDto(int Id,
                                decimal Net,
                                decimal Gross,
                                decimal Vat,
                                int VatRate);