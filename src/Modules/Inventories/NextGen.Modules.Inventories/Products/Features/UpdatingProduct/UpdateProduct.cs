using Ardalis.GuardClauses;
using BuildingBlocks.Abstractions.CQRS.Command;
using BuildingBlocks.Core.Exception;
using NextGen.Modules.Inventories.Brands.Exceptions.Application;
using NextGen.Modules.Inventories.Categories.Exceptions.Application;
using NextGen.Modules.Inventories.Products.Exceptions.Application;
using NextGen.Modules.Inventories.Products.Models;
using NextGen.Modules.Inventories.Products.ValueObjects;
using NextGen.Modules.Inventories.Shared.Contracts;
using NextGen.Modules.Inventories.Shared.Extensions;
using NextGen.Modules.Inventories.Suppliers.Exceptions.Application;
using FluentValidation;

namespace NextGen.Modules.Inventories.Products.Features.UpdatingProduct;

public record UpdateProduct(
    long Id,
    string Name,
    decimal Price,
    int RestockThreshold,
    int MaxStockThreshold,
    ProductStatus Status,
    int Width,
    int Height,
    int Depth,
    string Size,
    long CategoryId,
    long SupplierId,
    long BrandId,
    string? Description = null) : ITxUpdateCommand;

internal class UpdateProductValidator : AbstractValidator<UpdateProduct>
{
    public UpdateProductValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}

internal class UpdateProductCommandHandler : ICommandHandler<UpdateProduct>
{
    private readonly IInventoryDbContext _inventoryDbContext;

    public UpdateProductCommandHandler(IInventoryDbContext inventoryDbContext)
    {
        _inventoryDbContext = inventoryDbContext;
    }

    public async Task<Unit> Handle(UpdateProduct command, CancellationToken cancellationToken)
    {
        Guard.Against.Null(command, nameof(command));

        var product = await _inventoryDbContext.FindProductByIdAsync(command.Id);
        Guard.Against.NotFound(product, new ProductNotFoundException(command.Id));

        var category = await _inventoryDbContext.FindCategoryAsync(command.CategoryId);
        Guard.Against.NotFound(category, new CategoryNotFoundException(command.CategoryId));

        var brand = await _inventoryDbContext.FindBrandAsync(command.BrandId);
        Guard.Against.NotFound(brand, new BrandNotFoundException(command.BrandId));

        var supplier = await _inventoryDbContext.FindSupplierByIdAsync(command.SupplierId);
        Guard.Against.NotFound(supplier, new SupplierNotFoundException(command.SupplierId));

        product!.ChangeCategory(command.CategoryId);
        product.ChangeBrand(command.BrandId);
        product.ChangeSupplier(command.SupplierId);

        product.ChangeDescription(command.Description);
        product.ChangeName(command.Name);
        product.ChangePrice(command.Price);
        product.ChangeSize(command.Size);
        product.ChangeStatus(command.Status);
        product.ChangeDimensions(Dimensions.Create(command.Width, command.Height, command.Depth));
        product.ChangeMaxStockThreshold(command.MaxStockThreshold);
        product.ChangeRestockThreshold(command.RestockThreshold);

        await _inventoryDbContext.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
