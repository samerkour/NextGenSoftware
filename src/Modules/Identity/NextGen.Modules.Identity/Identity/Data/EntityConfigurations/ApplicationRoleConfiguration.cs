using NextGen.Modules.Identity.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NextGen.Modules.Identity.Identity.Data.EntityConfigurations;

internal class ApplicationRoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
{
    public void Configure(EntityTypeBuilder<ApplicationRole> builder)
    {
        builder.ToTable("AspNetRoles");

        builder.HasMany(e => e.UserRoles)
            .WithOne(e => e.Role)
            .HasForeignKey(ur => ur.RoleId)
            .IsRequired();

        builder.HasOne(r => r.RoleGroup)
            .WithMany(rg => rg.Roles)
            .HasForeignKey(r => r.RoleGroupId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
