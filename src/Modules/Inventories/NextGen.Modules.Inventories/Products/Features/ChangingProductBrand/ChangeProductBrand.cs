using BuildingBlocks.Abstractions.CQRS.Command;

namespace NextGen.Modules.Inventories.Products.Features.ChangingProductBrand;

internal record ChangeProductBrand : ITxCommand<ChangeProductBrandResponse>;

internal class ChangeProductBrandHandler :
    ICommandHandler<ChangeProductBrand, ChangeProductBrandResponse>
{
    public Task<ChangeProductBrandResponse> Handle(
        ChangeProductBrand command,
        CancellationToken cancellationToken)
    {
       return Task.FromResult<ChangeProductBrandResponse>(null!);
    }
}
