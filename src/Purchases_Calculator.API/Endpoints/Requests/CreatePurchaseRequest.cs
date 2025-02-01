namespace Purchases_Calculator.API.Endpoints.Requests;

public record CreatePurchaseRequest(decimal? Net, 
                                    decimal? Gross, 
                                    decimal? Vat, 
                                    int VatRate);