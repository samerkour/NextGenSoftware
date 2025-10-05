using Asp.Versioning.Conventions;
using BuildingBlocks.Abstractions.Web;
using Swashbuckle.AspNetCore.Annotations;

namespace NextGen.Modules.Inventories.Products.Features.ReplenishingProductStock;

// POST api/v1/inventory/products/{productId}/replenish-stock
public static class ReplenishingProductStockEndpoint
{
    internal static IEndpointRouteBuilder MapReplenishProductStockEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
                $"{ProductsConfigs.ProductsPrefixUri}/{{productId}}/replenish-stock",
                ReplenishProductStock)
            .RequireAuthorization()
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .WithTags(ProductsConfigs.Tag)
            .WithMetadata(new SwaggerOperationAttribute(
                "Replenishing ProductStock Products ",
                "Replenishing ProductStock Products"))
            .WithName("ReplenishProductStock")
            .WithDisplayName("Replenish product stock")
            .WithApiVersionSet(ProductsConfigs.VersionSet)
            .HasApiVersion(1.0);

        return endpoints;
    }

    private static Task<IResult> ReplenishProductStock(
        long productId,
        int quantity,
        IGatewayProcessor<InventoryModuleConfiguration> gatewayProcessor,
        CancellationToken cancellationToken)
    {
        return gatewayProcessor.ExecuteCommand(async commandProcessor =>
        {
            await commandProcessor.SendAsync(new ReplenishingProductStock(productId, quantity), cancellationToken);

            return Results.NoContent();
        });
    }
}
