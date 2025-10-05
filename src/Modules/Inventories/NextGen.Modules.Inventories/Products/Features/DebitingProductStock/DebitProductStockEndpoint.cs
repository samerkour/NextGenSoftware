using Asp.Versioning.Conventions;
using BuildingBlocks.Abstractions.Web;
using Swashbuckle.AspNetCore.Annotations;

namespace NextGen.Modules.Inventories.Products.Features.DebitingProductStock;

// POST api/v1/inventory/products/{productId}/debit-stock
public static class DebitProductStockEndpoint
{
    internal static IEndpointRouteBuilder MapDebitProductStockEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
                $"{ProductsConfigs.ProductsPrefixUri}/{{productId}}/debit-stock",
                DebitProductStock)
            .WithTags(ProductsConfigs.Tag)
            .RequireAuthorization()
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .WithTags(ProductsConfigs.Tag)
            .WithMetadata(new SwaggerOperationAttribute("Debiting Product Stock", "Debiting Product Stock"))
            .WithName("DebitProductStock")
            .WithDisplayName("Debit product stock")
            .WithApiVersionSet(ProductsConfigs.VersionSet)
            .HasApiVersion(1.0);

        return endpoints;
    }

    private static Task<IResult> DebitProductStock(
        long productId,
        int quantity,
        IGatewayProcessor<InventoryModuleConfiguration> gatewayProcessor,
        CancellationToken cancellationToken)
    {
        return gatewayProcessor.ExecuteCommand(async commandProcessor =>
        {
            await commandProcessor.SendAsync(new DebitProductStock(productId, quantity), cancellationToken);

            return Results.NoContent();
        });
    }
}
