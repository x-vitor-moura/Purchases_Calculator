namespace Purchases_Calculator.API.Endpoints.Responses;

public record GetByIdPurchaseDetails(int Id,
                                     string PurchaseRegistrationDate,
                                     decimal Net,
                                     decimal Gross,
                                     decimal Vat,
                                     int VatRate);