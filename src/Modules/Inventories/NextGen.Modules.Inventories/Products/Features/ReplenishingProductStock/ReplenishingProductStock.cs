using Ardalis.GuardClauses;
using BuildingBlocks.Abstractions.CQRS.Command;
using BuildingBlocks.Core.Exception;
using NextGen.Modules.Inventories.Products.Exceptions.Application;
using NextGen.Modules.Inventories.Shared.Contracts;
using NextGen.Modules.Inventories.Shared.Extensions;
using FluentValidation;

namespace NextGen.Modules.Inventories.Products.Features.ReplenishingProductStock;

public record ReplenishingProductStock(long ProductId, int Quantity) : ITxCommand;

internal class ReplenishingProductStockValidator : AbstractValidator<ReplenishingProductStock>
{
    public ReplenishingProductStockValidator()
    {
        // https://docs.fluentvalidation.net/en/latest/conditions.html#stop-vs-stoponfirstfailure
        CascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Quantity)
            .GreaterThan(0);

        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("ProductId must be greater than 0");
    }
}

internal class ReplenishingProductStockHandler : ICommandHandler<ReplenishingProductStock>
{
    private readonly IInventoryDbContext _inventoryDbContext;

    public ReplenishingProductStockHandler(IInventoryDbContext inventoryDbContext)
    {
        _inventoryDbContext = Guard.Against.Null(inventoryDbContext, nameof(inventoryDbContext));
    }

    public async Task<Unit> Handle(ReplenishingProductStock command, CancellationToken cancellationToken)
    {
        Guard.Against.Null(command, nameof(command));

        var product = await _inventoryDbContext.FindProductByIdAsync(command.ProductId);
        Guard.Against.NotFound(product, new ProductNotFoundException(command.ProductId));

        product!.ReplenishStock(command.Quantity);
        await _inventoryDbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
