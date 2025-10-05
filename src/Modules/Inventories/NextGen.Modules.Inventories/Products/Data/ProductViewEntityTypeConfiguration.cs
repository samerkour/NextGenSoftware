using NextGen.Modules.Inventories.Products.Models;
using NextGen.Modules.Inventories.Shared.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NextGen.Modules.Inventories.Products.Data;

public class ProductViewEntityTypeConfiguration : IEntityTypeConfiguration<ProductView>
{
    public void Configure(EntityTypeBuilder<ProductView> builder)
    {
        builder.ToTable("product_views", InventoryDbContext.DefaultSchema);
        builder.HasKey(x => x.ProductId);
        builder.HasIndex(x => x.ProductId).IsUnique();
    }
}
