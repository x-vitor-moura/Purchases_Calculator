using Purchases_Calculator.API.Application;
using Purchases_Calculator.API.Common.Extensions;
using Purchases_Calculator.API.Endpoints.Requests;
using Purchases_Calculator.API.Endpoints.Responses;
using Purchases_Calculator.API.Infrastructure.Databases.Repositories;
using Purchases_Calculator.API.Infrastructure.Messaging;

namespace Purchases_Calculator.API.Endpoints.APIs;

public class CreatePurchase : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) => app
    .MapPost("/purchase", Handle)
    .WithSummary("Calculates Purchase Net amount, Gross amount, and Vat amount")
    .WithRequestValidation<CreatePurchaseRequest>();

    public static async Task<IResult> Handle(CreatePurchaseRequest request,
                                             IMessagePublisher messagePublisher,
                                             IPurchaseRepository purchaseRepository,
                                             CancellationToken cancellationToken)
    {
        // Use calculator to calculate Net, Gross and Vat
        var purchase = Calculator.CalculatePurchase(request);

        // Save the purchase and publish a message
        await purchaseRepository.CreateAsync(purchase);

        await messagePublisher.PublishMessageAsync("purchase",purchase);

        return Results.Created<CreatePurchaseDto>($"/gb/purchase/{purchase.Id}", new CreatePurchaseDto(purchase.Id,
                                                purchase.Net,
                                                purchase.Gross,
                                                purchase.Vat,
                                                purchase.VatRate)
        );
    }
}
