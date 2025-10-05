using BuildingBlocks.Abstractions.CQRS.Command;

namespace NextGen.Modules.Inventories.Products.Features.ChangingProductCategory;

internal record ChangeProductCategory : ITxCommand<ChangeProductCategoryResponse>;

internal class ChangeProductCategoryHandler :
    ICommandHandler<ChangeProductCategory, ChangeProductCategoryResponse>
{
    public Task<ChangeProductCategoryResponse> Handle(
        ChangeProductCategory command,
        CancellationToken cancellationToken)
    {
        return Task.FromResult<ChangeProductCategoryResponse>(null!);
    }
}
