using Purchases_Calculator.API.Domain;
using Purchases_Calculator.API.Endpoints.Requests;

namespace Purchases_Calculator.API.Application;

public static class Calculator
{
    public static Purchase CalculatePurchase(CreatePurchaseRequest request)
    {
        decimal vatRate = request.VatRate / 100m;

        decimal net = 0;
        decimal gross = 0;
        decimal vat = 0;

        if (request.Net.HasValue && request.Net != 0)
        {
            net = RoundToTwoDecimalPlaces(request.Net.Value);
            vat = RoundToTwoDecimalPlaces(net * vatRate);
            gross = RoundToTwoDecimalPlaces(net + vat);
        }
        else if (request.Gross.HasValue && request.Gross != 0)
        {
            gross = RoundToTwoDecimalPlaces(request.Gross.Value);
            net = RoundToTwoDecimalPlaces(gross / (1 + vatRate));
            vat = RoundToTwoDecimalPlaces(gross - net);
        }
        else if (request.Vat.HasValue && request.Vat != 0)
        {
            vat = RoundToTwoDecimalPlaces(request.Vat.Value);
            gross = RoundToTwoDecimalPlaces(vat / vatRate * (1 + vatRate));
            net = RoundToTwoDecimalPlaces(gross - vat);
        }

        return new Purchase(net, gross, vat, request.VatRate);
    }

    private static decimal RoundToTwoDecimalPlaces(decimal value)
    {
        return Math.Round(value, 2);
    }
}
