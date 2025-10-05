using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NextGen.Modules.Identity.Shared.Models;

namespace NextGen.Modules.Identity.Identity.Data.EntityConfigurations;
internal class RoleGroupConfiguration : IEntityTypeConfiguration<RoleGroup>
{
    public void Configure(EntityTypeBuilder<RoleGroup> builder)
    {
        builder.ToTable("RoleGroups");
        builder.HasKey(rg => rg.Id);
        builder.Property(rg => rg.Name).HasMaxLength(200).IsRequired();

        builder.HasOne(rg => rg.Module)
               .WithMany(m => m.RoleGroups)
               .HasForeignKey(rg => rg.ModuleId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}

