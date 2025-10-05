using Asp.Versioning.Conventions;
using BuildingBlocks.Abstractions.Web;
using Swashbuckle.AspNetCore.Annotations;

namespace NextGen.Modules.Inventories.Products.Features.GettingProductsView;

// GET api/v1/inventory/products
public static class GetProductsViewEndpoint
{
    internal static IEndpointRouteBuilder MapGetProductsViewEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                $"{InventoryModuleConfiguration.InventoryModulePrefixUri}/products-view/{{page}}/{{pageSize}}",
                GetProductsView)
            // .RequireAuthorization()
            .Produces<GetProductsViewResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status400BadRequest)
            .WithDisplayName("Get Products View.")
            .WithName("GetProductsView")
            .WithTags(ProductsConfigs.Tag)
            .WithMetadata(new SwaggerOperationAttribute("Getting Products View", "Getting Products View"))
            .WithApiVersionSet(ProductsConfigs.VersionSet)
            .HasApiVersion(1.0);

        return endpoints;
    }

    private static Task<IResult> GetProductsView(
        IGatewayProcessor<InventoryModuleConfiguration> gatewayProcessor,
        CancellationToken cancellationToken,
        int page = 1,
        int pageSize = 20)
    {
        return gatewayProcessor.ExecuteQuery(async queryProcessor =>
        {
            var result = await queryProcessor.SendAsync(
                new GetProductsView {Page = page, PageSize = pageSize},
                cancellationToken);

            return Results.Ok(result);
        });
    }
}
