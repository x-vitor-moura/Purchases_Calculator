namespace Purchases_Calculator.API.Endpoints.Responses;

public record GetAllPurchaseDto(int Id,
                                decimal Net,
                                decimal Gross,
                                decimal Vat,      
                                int VatRate);