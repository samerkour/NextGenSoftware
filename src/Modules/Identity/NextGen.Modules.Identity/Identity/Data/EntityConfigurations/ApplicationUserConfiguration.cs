using BuildingBlocks.Core.Persistence.EfCore;
using NextGen.Modules.Identity.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NextGen.Modules.Identity.Identity.Data.EntityConfigurations;

internal class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        // Basic string properties
        builder.Property(x => x.FirstName).HasMaxLength(100).IsRequired();
        builder.Property(x => x.LastName).HasMaxLength(100).IsRequired();
        builder.Property(x => x.MiddleName).HasMaxLength(100).IsRequired(false);
        builder.Property(x => x.PlaceOfBirth).HasMaxLength(100).IsRequired(false);
        builder.Property(x => x.ProfileImagePath).HasMaxLength(255).IsRequired(false);

        // Location / Address fields
        builder.Property(x => x.Country).HasMaxLength(100).IsRequired(false);
        builder.Property(x => x.City).HasMaxLength(100).IsRequired(false);
        builder.Property(x => x.State).HasMaxLength(100).IsRequired(false);
        builder.Property(x => x.Address).HasMaxLength(250).IsRequired(false);
        builder.Property(x => x.PostalCode).HasMaxLength(20).IsRequired(false);

        builder.Property(x => x.UserName).HasMaxLength(50).IsRequired();
        builder.Property(x => x.NormalizedUserName).HasMaxLength(50).IsRequired();
        builder.Property(x => x.Email).HasMaxLength(50).IsRequired();
        builder.Property(x => x.NormalizedEmail).HasMaxLength(50).IsRequired();
        builder.Property(x => x.PhoneNumber).HasMaxLength(15).IsRequired(false);

        // Date properties
        builder.Property(x => x.CreatedAt).HasDefaultValueSql(EfConstants.DateAlgorithm);
        builder.Property(x => x.DateOfBirth).IsRequired(false);
        builder.Property(x => x.PasswordLastChangedOn).IsRequired();
        builder.Property(x => x.TwoFactorEnabledOn).IsRequired(false);
        builder.Property(x => x.LockoutEnabledOn).IsRequired(false);
        builder.Property(x => x.DeletedOn).IsRequired(false);

        // Indexes
        builder.HasIndex(x => x.Email).IsUnique();
        builder.HasIndex(x => x.NormalizedEmail).IsUnique();

        // Navigation: UserRoles
        builder.HasMany(e => e.UserRoles)
            .WithOne(e => e.User)
            .HasForeignKey(ur => ur.UserId)
            .IsRequired();

        // Navigation: RefreshTokens
        builder.HasMany(e => e.RefreshTokens)
            .WithOne(rt => rt.ApplicationUser)
            .HasForeignKey(rt => rt.UserId)
            .IsRequired();
    }
}
