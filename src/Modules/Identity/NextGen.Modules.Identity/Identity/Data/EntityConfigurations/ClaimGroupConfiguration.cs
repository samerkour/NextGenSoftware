using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NextGen.Modules.Identity.Identity.Data.EntityConfigurations;
internal class ClaimGroupConfiguration : IEntityTypeConfiguration<ClaimGroup>
{
    public void Configure(EntityTypeBuilder<ClaimGroup> builder)
    {
        builder.ToTable("ClaimGroups");
        builder.HasKey(cg => cg.Id);
        builder.Property(cg => cg.Name).HasMaxLength(200).IsRequired();

        // many-to-many Role <-> ClaimGroup
        builder.HasMany(cg => cg.Roles)
               .WithMany(r => r.ClaimGroups)
               .UsingEntity(j => j.ToTable("RoleClaimGroups"));
    }
}
