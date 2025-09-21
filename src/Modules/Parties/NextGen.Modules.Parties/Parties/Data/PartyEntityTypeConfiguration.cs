using BuildingBlocks.Core.Domain.ValueObjects;
using BuildingBlocks.Core.Persistence.EfCore;
using NextGen.Modules.Parties.Parties.Models;
using NextGen.Modules.Parties.Parties.ValueObjects;
using NextGen.Modules.Parties.Shared.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NextGen.Modules.Parties.Parties.Data;

public class PartyEntityTypeConfiguration : IEntityTypeConfiguration<Party>
{
    public void Configure(EntityTypeBuilder<Party> builder)
    {
        builder.ToTable("parties", PartiesDbContext.DefaultSchema);

        builder.HasKey(c => c.Id);
        builder.HasIndex(x => x.Id).IsUnique();

        builder.Property(x => x.Id)
            .HasConversion(id => id.Value, id => new PartyId(id))
            .ValueGeneratedNever();

        builder.HasIndex(x => x.IdentityId).IsUnique();

        builder.HasIndex(x => x.Email).IsUnique();
        builder.Property(x => x.Email).IsRequired()
            .HasMaxLength(EfConstants.Lenght.Medium)
            .HasConversion(email => email.Value, email => Email.Create(email)); // converting mandatory value objects

        builder.HasIndex(x => x.PhoneNumber).IsUnique();
        builder.Property(x => x.PhoneNumber)
            .IsRequired(false)
            .HasMaxLength(EfConstants.Lenght.Tiny)
            .HasConversion(x => (string?)x, x => (PhoneNumber?)x);

        builder.OwnsOne(m => m.Name, a =>
        {
            a.Property(p => p.FirstName)
                .HasMaxLength(EfConstants.Lenght.Medium);

            a.Property(p => p.LastName)
                .HasMaxLength(EfConstants.Lenght.Medium);

            a.Ignore(p => p.FullName);
        });

        builder.OwnsOne(m => m.Address, a =>
        {
            a.Property(p => p.City)
                .HasMaxLength(EfConstants.Lenght.Short);

            a.Property(p => p.Country)
                .HasMaxLength(EfConstants.Lenght.Medium);

            a.Property(p => p.Detail)
                .HasMaxLength(EfConstants.Lenght.Medium);
        });

        builder.Property(x => x.Nationality)
            .IsRequired(false)
            .HasMaxLength(EfConstants.Lenght.Short)
            .HasConversion(
                nationality => (string?)nationality,
                nationality => (Nationality?)nationality); // converting optional value objects

        builder.Property(x => x.BirthDate)
            .IsRequired(false)
            .HasConversion(x => (DateTime?)x, x => (BirthDate?)x);

        builder.Property(x => x.Created).HasDefaultValueSql(EfConstants.DateAlgorithm);
    }
}
