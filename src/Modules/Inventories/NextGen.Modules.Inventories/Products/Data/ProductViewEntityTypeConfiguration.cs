using NextGen.Modules.Inventorys.Products.Models;
using NextGen.Modules.Inventorys.Shared.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NextGen.Modules.Inventorys.Products.Data;

public class ProductViewEntityTypeConfiguration : IEntityTypeConfiguration<ProductView>
{
    public void Configure(EntityTypeBuilder<ProductView> builder)
    {
        builder.ToTable("product_views", InventoryDbContext.DefaultSchema);
        builder.HasKey(x => x.ProductId);
        builder.HasIndex(x => x.ProductId).IsUnique();
    }
}
