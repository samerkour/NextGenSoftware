using BuildingBlocks.Core.Persistence.EfCore;
using NextGen.Modules.Notifications.Notifications.Models;
using NextGen.Modules.Notifications.Shared.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NextGen.Modules.Notifications.Notifications.Data.EntityConfigurations;

public class NotificationEntityTypeConfiguration : IEntityTypeConfiguration<Notif>
{
    public void Configure(EntityTypeBuilder<Notif> builder)
    {
        builder.ToTable("notifs", NotificationsDbContext.DefaultSchema);

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
