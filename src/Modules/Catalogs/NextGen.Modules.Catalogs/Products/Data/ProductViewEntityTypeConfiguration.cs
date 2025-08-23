using NextGen.Modules.Catalogs.Products.Models;
using NextGen.Modules.Catalogs.Shared.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NextGen.Modules.Catalogs.Products.Data;

public class ProductViewEntityTypeConfiguration : IEntityTypeConfiguration<ProductView>
{
    public void Configure(EntityTypeBuilder<ProductView> builder)
    {
        builder.ToTable("product_views", CatalogDbContext.DefaultSchema);
        builder.HasKey(x => x.ProductId);
        builder.HasIndex(x => x.ProductId).IsUnique();
    }
}
