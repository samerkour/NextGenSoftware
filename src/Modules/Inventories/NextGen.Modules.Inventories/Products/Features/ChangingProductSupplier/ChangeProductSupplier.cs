using BuildingBlocks.Abstractions.CQRS.Command;

namespace NextGen.Modules.Inventories.Products.Features.ChangingProductSupplier;

internal record ChangeProductSupplier : ITxCommand<ChangeProductSupplierResponse>;

internal class ChangeProductSupplierCommandHandler :
    ICommandHandler<ChangeProductSupplier, ChangeProductSupplierResponse>
{
    public Task<ChangeProductSupplierResponse> Handle(
        ChangeProductSupplier request,
        CancellationToken cancellationToken)
    {
        return Task.FromResult<ChangeProductSupplierResponse>(null!);
    }
}
