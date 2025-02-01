using Purchases_Calculator.API.Common.Filters;
using Purchases_Calculator.API.Endpoints.APIs;

namespace Purchases_Calculator.API.Endpoints;

public static class EndpointHandlers
{
    public static void MapEndpoints(this WebApplication app)
    {
        app.MapGroup("")
            .AddEndpointFilter<RequestLoggingFilter>()
            .WithOpenApi()
            .MapPurchaseEndpoints();
    }

    private static void MapPurchaseEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGroup("/gb")
            .WithTags("global blue calculator")
            .AllowAnonymous()
            .MapEndpoint<CreatePurchase>()
            .MapEndpoint<GetByIdPurchase>()
            .MapEndpoint<GetAllPurchase>();
    }

    private static IEndpointRouteBuilder MapEndpoint<TEndpoint>(this IEndpointRouteBuilder app) where TEndpoint : IEndpoint
    {
        TEndpoint.Map(app);
        return app;
    }
}
