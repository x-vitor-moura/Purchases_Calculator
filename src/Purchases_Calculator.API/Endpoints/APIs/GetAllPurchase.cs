using Purchases_Calculator.API.Endpoints.Responses;
using Purchases_Calculator.API.Infrastructure.Databases.Repositories;

namespace Purchases_Calculator.API.Endpoints.APIs;

public class GetAllPurchase : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) => app
    .MapGet("/purchase", Handle)
    .WithSummary("Gets a all purchases");

    public static async Task<IResult> Handle(IPurchaseRepository purchaseRepository, CancellationToken cancellationToken)
    {
        var result = await purchaseRepository.GetAllAsync();

        var purchases = result.Select(p => new GetAllPurchaseDto(p.Id, p.Net, p.Gross, p.Vat, p.VatRate));

        return Results.Ok(purchases);
    }
}