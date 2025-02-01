using Purchases_Calculator.API.Endpoints.Requests;
using Purchases_Calculator.API.Endpoints.Responses;
using Purchases_Calculator.API.Infrastructure.Databases.Repositories;

namespace Purchases_Calculator.API.Endpoints.APIs;

public class GetByIdPurchase : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) => app
    .MapGet("/purchase/{id}", Handle)
    .WithSummary("Gets a purchase by id");

    public static async Task<IResult> Handle([AsParameters] GetByIdPurchaseRequest request,
                                                                       IPurchaseRepository purchaseRepository,
                                                                       CancellationToken cancellationToken)
    {
        var result = await purchaseRepository.GetByIdAsync(request.Id);

        if (result is null)
        {
            return Results.NotFound($"Purchase with Id: {request.Id} not Found");
        }

        var purchaseDetails = new GetByIdPurchaseDetails(result.Id,
                                                         result.PurchaseRegistrationDateString,
                                                         result.Net,
                                                         result.Gross,
                                                         result.Vat,
                                                         result.VatRate);
        return Results.Ok(purchaseDetails);
    }
}
