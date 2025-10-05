using BuildingBlocks.Core.Persistence.EfCore;
using NextGen.Modules.Sales.Sales.Models;
using NextGen.Modules.Sales.Shared.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NextGen.Modules.Sales.Sales.Data.EntityConfigurations;

public class SaleEntityTypeConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("orders", SalesDbContext.DefaultSchema);

        builder.HasKey(c => c.Id);
        builder.HasIndex(x => x.Id).IsUnique();

        builder.Property(x => x.Id)
            .HasConversion(x => x.Value, id => id)
            .ValueGeneratedNever();

        builder.OwnsOne(m => m.Party, a =>
        {
            a.Property(p => p.Name)
                .HasMaxLength(EfConstants.Lenght.Medium);

            a.Property(p => p.PartyId);
        });

        builder.OwnsOne(m => m.Product, a =>
        {
            a.Property(p => p.Name)
                .HasMaxLength(EfConstants.Lenght.Medium);

            a.Property(p => p.Price);

            a.Property(p => p.ProductId);
        });
    }
}
