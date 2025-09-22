using Ardalis.GuardClauses;
using BuildingBlocks.Abstractions.CQRS.Command;
using BuildingBlocks.Core.Exception;
using NextGen.Modules.Inventories.Products.Exceptions.Application;
using NextGen.Modules.Inventories.Shared.Contracts;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace NextGen.Modules.Inventories.Products.Features.DebitingProductStock;

public record DebitProductStock(long ProductId, int Quantity) : ITxCommand;

internal class DebitProductStockValidator : AbstractValidator<DebitProductStock>
{
    public DebitProductStockValidator()
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

internal class DebitProductStockHandler : ICommandHandler<DebitProductStock>
{
    private readonly IInventoryDbContext _inventoryDbContext;

    public DebitProductStockHandler(IInventoryDbContext inventoryDbContext)
    {
        _inventoryDbContext = Guard.Against.Null(inventoryDbContext, nameof(inventoryDbContext));
    }

    public async Task<Unit> Handle(DebitProductStock command, CancellationToken cancellationToken)
    {
        Guard.Against.Null(command, nameof(command));

        var product =
            await _inventoryDbContext.Products.FirstOrDefaultAsync(x => x.Id == command.ProductId, cancellationToken);

        await _inventoryDbContext.SaveChangesAsync(cancellationToken);
        Guard.Against.NotFound(product, new ProductNotFoundException(command.ProductId));
        product!.DebitStock(command.Quantity);

        await _inventoryDbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
