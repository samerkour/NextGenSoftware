using BuildingBlocks.Core.Domain.ValueObjects;
using BuildingBlocks.Core.Persistence.EfCore;
using NextGen.Modules.Parties.Parties.Models;
using NextGen.Modules.Parties.RestockSubscriptions.Models.Write;
using NextGen.Modules.Parties.Shared.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NextGen.Modules.Parties.RestockSubscriptions.Data;

public class RestockSubscriptionEntityConfiguration : IEntityTypeConfiguration<RestockSubscription>
{
    public void Configure(EntityTypeBuilder<RestockSubscription> builder)
    {
        builder.ToTable("restock_subscriptions", PartiesDbContext.DefaultSchema);

        builder.HasKey(c => c.Id);
        builder.HasIndex(x => x.Id).IsUnique();
        builder.Property(x => x.Id)
            .ValueGeneratedNever()
            .HasConversion(id => id.Value, id => id);

        builder.Property(x => x.Processed).HasDefaultValue(false);

        builder.Property(c => c.PartyId)
            .HasConversion(id => id.Value, id => id);

        builder.HasOne<Party>()
            .WithMany()
            .HasForeignKey(x => x.PartyId);

        builder.OwnsOne(x => x.ProductInformation, p =>
        {
            p.Property(x => x.Id)
                .HasConversion(id => id.Value, id => id);

            p.Property(x => x.Name)
                .HasMaxLength(EfConstants.Lenght.Normal);
        });

        builder.Property(x => x.Email)
            .HasConversion(email => email.Value, email => Email.Create(email));

        builder.Property(x => x.Created).HasDefaultValueSql(EfConstants.DateAlgorithm);
    }
}
